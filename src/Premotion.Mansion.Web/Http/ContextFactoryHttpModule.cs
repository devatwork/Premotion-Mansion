﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements <see cref="IHttpModule"/> for mansion web application.
	/// </summary>
	public class ContextFactoryHttpModule : DisposableBase, IHttpModule
	{
		#region Constants
		/// <summary>
		/// Key under which the <see cref="IMansionWebContext"/> is stored in the <see cref="HttpContextBase.Items"/> collection.
		/// </summary>
		private const string RequestContextKey = "mansion-request-context";
		#endregion
		#region Nested type: MansionWebContext
		/// <summary>
		/// Implements the <see cref="IMansionWebContext"/>.
		/// </summary>
		private class MansionWebContext : MansionContext, IMansionWebContext
		{
			#region Constructors
			private MansionWebContext(INucleus nucleus, HttpContextBase httpContext, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(nucleus)
			{
				// validate arguments
				if (httpContext == null)
					throw new ArgumentNullException("httpContext");
				if (initialStack == null)
					throw new ArgumentNullException("initialStack");

				// set value
				this.httpContext = httpContext;
				applicationBaseUri = new UriBuilder(httpContext.Request.Url)
				                     {
				                     	Fragment = String.Empty,
				                     	Query = String.Empty,
				                     	Path = (httpContext.Request.ApplicationPath ?? String.Empty).Trim('/') + "/"
				                     }.Uri;

				// create thet stack
				Stack = new AutoPopDictionaryStack<string, IPropertyBag>(StringComparer.OrdinalIgnoreCase, initialStack);

				// extracts needed dataspaces
				Stack.Push("Get", httpContext.Request.QueryString.ToPropertyBag(), true);
				Stack.Push("Post", httpContext.Request.Form.ToPropertyBag(), true);
				Stack.Push("Server", httpContext.Request.ServerVariables.ToPropertyBag(), true);

				// get the original url
				var originalUri = PathRewriterHttpModule.GetOriginalRawUrl(httpContext);
				originalUri = WebUtilities.StripPort(originalUri);

				// create the application dataspace
				Stack.Push("Request", new PropertyBag
				                      {
				                      	{"url", originalUri.ToString()},
				                      	{"urlPath", httpContext.Request.Url.GetLeftPart(UriPartial.Path)},
				                      	{"baseUrl", ApplicationBaseUri.ToString().TrimEnd('/')}
				                      }, true);

				// set context location flag
				IsBackoffice = HttpContext.Request.Path.IndexOf(@"/cms/", HttpContext.Request.ApplicationPath.Length, StringComparison.OrdinalIgnoreCase) != -1;
			}
			#endregion
			#region Factory Methods
			/// <summary>
			/// Constructs an instance of <see cref="MansionWebContext"/>.
			/// </summary>
			/// <param name="applicationContext">The global application context.</param>
			/// <param name="httpContext">The <see cref="HttpContextBase"/> of the current request.</param>
			/// <returns>Returns the constructed <see cref="IMansionWebContext"/>.</returns>
			public static IMansionWebContext Create(IMansionContext applicationContext, HttpContextBase httpContext)
			{
				// validate arguments
				if (applicationContext == null)
					throw new ArgumentNullException("applicationContext");
				if (httpContext == null)
					throw new ArgumentNullException("httpContext");

				// create the context
				return new MansionWebContext(applicationContext.Nucleus, httpContext, applicationContext.Stack);
			}
			#endregion
			#region Implementation of IMansionWebContext
			/// <summary>
			/// Generates a new control ID.
			/// </summary>
			/// <returns>Returns the generated control id.</returns>
			public string GetNextControlId()
			{
				return (controlIdGenerator++).ToString();
			}
			/// <summary>
			/// Tries to get the top most control.
			/// </summary>
			/// <typeparam name="TControl">The type of <see cref="Control"/> which to get.</typeparam>
			/// <param name="control">The found control.</param>
			/// <returns>Returns true when the control was found, otherwise false.</returns>
			public bool TryPeekControl<TControl>(out TControl control) where TControl : class, IControl
			{
				IControl untypedControl;
				if (!ControlStack.TryPeek(out untypedControl))
				{
					control = default(TControl);
					return false;
				}

				// try to cast the control
				control = untypedControl as TControl;
				return control != null;
			}
			/// <summary>
			/// Tries to find a control in the <see cref="ControlStack"/>.
			/// </summary>
			/// <typeparam name="TControl">The type of <see cref="Control"/> which to find.</typeparam>
			/// <param name="control">The found control.</param>
			/// <returns>Returns true when the control was found, otherwise false.</returns>
			public bool TryFindControl<TControl>(out TControl control) where TControl : class, IControl
			{
				// loop over the controls
				foreach (var candidate in ControlStack.OfType<TControl>())
				{
					control = candidate;
					return true;
				}
				control = default(TControl);
				return false;
			}
			/// <summary>
			/// Gets the top most <see cref="MailMessage"/> from the <see cref="MessageStack"/>.
			/// </summary>
			public MailMessage Message
			{
				get
				{
					// check if there is no message on the stack
					MailMessage message;
					if (!MessageStack.TryPeek(out message))
						throw new InvalidOperationException("No message was found on the stack");

					return message;
				}
			}
			/// <summary>
			/// Gets the message stack.
			/// </summary>
			public IAutoPopStack<MailMessage> MessageStack
			{
				get { return messageStack; }
			}
			/// <summary>
			/// Gets the <see cref="HttpContextBase"/>.
			/// </summary>
			public HttpContextBase HttpContext
			{
				get { return httpContext; }
			}
			/// <summary>
			/// Gets the application <see cref="Uri"/>.
			/// </summary>
			public Uri ApplicationBaseUri
			{
				get { return applicationBaseUri; }
			}
			/// <summary>
			/// Gets the <see cref="Control"/> stack.
			/// </summary>
			public IAutoPopStack<IControl> ControlStack
			{
				get { return controlStack; }
			}
			/// <summary>
			/// Gets the <see cref="Form"/> stack.
			/// </summary>
			public IAutoPopStack<Form> FormStack
			{
				get { return formStack; }
			}
			/// <summary>
			/// Get the current <see cref="Form"/> from the <see cref="FormStack"/>.
			/// </summary>
			public Form CurrentForm
			{
				get
				{
					// check if there is no form on the stack
					Form form;
					if (!FormStack.TryPeek(out form))
						throw new InvalidOperationException("No form was found on the stack");

					return form;
				}
			}
			#endregion
			#region Private Fields
			private readonly Uri applicationBaseUri;
			private readonly IAutoPopStack<IControl> controlStack = new AutoPopStack<IControl>();
			private readonly IAutoPopStack<Form> formStack = new AutoPopStack<Form>();
			private readonly HttpContextBase httpContext;
			private readonly IAutoPopStack<MailMessage> messageStack = new AutoPopStack<MailMessage>();
			private int controlIdGenerator;
			#endregion
		}
		#endregion
		#region Implementation of IHttpModule
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
		public void Init(HttpApplication context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// hook into the request
			context.BeginRequest += (sender, args) =>
			                        {
			                        	// wrap the http context
			                        	var httpContext = new HttpContextWrapper(HttpContext.Current);

			                        	// initialize the mansion web context
			                        	var requestContext = MansionWebContext.Create(MansionHttpApplication.ApplicationContext, httpContext);

			                        	// store it in the request
			                        	httpContext.Items[RequestContextKey] = requestContext;
			                        };
			context.EndRequest += (sender, args) =>
			                      {
			                      	// get the mansion web request
			                      	var requestContext = RequestContext as MansionWebContext;
			                      	if (requestContext == null)
			                      		throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

			                      	// dispose the context properly
			                      	requestContext.Dispose();
			                      };
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the <see cref="IMansionWebContext"/> of the current request.
		/// </summary>
		/// <value> </value>
		/// <exception cref="InvalidOperationException"></exception>
		public static IMansionWebContext RequestContext
		{
			get
			{
				// get the request context
				var requestContext = HttpContext.Current.Items[RequestContextKey] as IMansionWebContext;
				if (requestContext == null)
					throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

				// return the context
				return requestContext;
			}
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
		}
		#endregion
	}
}