using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Security;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents the context for mansion web applications.
	/// </summary>
	public class MansionWebContext : MansionContext
	{
		#region Constructors
		/// <summary>
		/// Constructs a context extesion.
		/// </summary>
		/// <param name="nucleus">The original <see cref="INucleus"/> being extended.</param>
		/// <param name="httpContext">The <see cref="IHttpContext"/>.</param>
		/// <param name="initialStack">The initial stack.</param>
		private MansionWebContext(INucleus nucleus, IHttpContext httpContext, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(nucleus)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			if (initialStack == null)
				throw new ArgumentNullException("initialStack");

			// set value
			this.httpContext = httpContext;

			// create thet stack
			Stack = new AutoPopDictionaryStack<string, IPropertyBag>(StringComparer.OrdinalIgnoreCase, initialStack);

			// extracts needed dataspaces
			Stack.Push("Get", httpContext.Request.QueryString.ToPropertyBag(), true);
			Stack.Push("Post", httpContext.Request.Form.ToPropertyBag(), true);
			Stack.Push("Server", httpContext.Request.ServerVariables.ToPropertyBag(), true);

			// create the application dataspace
			Stack.Push("Request", new PropertyBag
			                      {
			                      	{"url", PathRewriterModule.GetOriginalRawUrl(httpContext).ToString()},
			                      	{"urlPath", httpContext.Request.Url.GetLeftPart(UriPartial.Path)},
			                      	{"baseUrl", httpContext.Request.ApplicationBaseUri.ToString().TrimEnd('/')}
			                      }, true);

			// set context location flag
			IsBackoffice = HttpContext.Request.Path.IndexOf(@"/cms/", HttpContext.Request.ApplicationPath.Length, StringComparison.OrdinalIgnoreCase) != -1;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates the web request.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/>.</param>
		/// <returns>Returns the created request.</returns>
		public static MansionWebContext Create(HttpContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// adap the HTTP context to something more useful
			var httpContext = HttpContextAdapter.Adapt(context);

			// create the context
			var mansionContext = new MansionWebContext(MansionHttpApplicationBase.ApplicationContext.Nucleus, httpContext, MansionHttpApplicationBase.GlobalData);

			// get the application dataspace
			IPropertyBag applicationDataspace;
			if (mansionContext.Stack.TryPeek("Application", out applicationDataspace))
			{
				// initialize the repository, when possible
				var repositoryNamespace = applicationDataspace.Get(mansionContext, "repositoryNamespace", string.Empty);
				var repositoryConnectionString = applicationDataspace.Get(mansionContext, "repositoryConnectionString", string.Empty);
				if (!string.IsNullOrEmpty(repositoryNamespace) && !string.IsNullOrEmpty(repositoryConnectionString))
				{
					// open the repository
					mansionContext.repositoryDisposable = RepositoryUtil.Open(mansionContext, repositoryNamespace, repositoryConnectionString);
				}

				// initialize the security context
				mansionContext.Nucleus.Get<ISecurityService>(mansionContext).InitializeSecurityContext(mansionContext);
			}

			// return the created context
			return mansionContext;
		}
		#endregion
		#region Control Methods
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
		#endregion
		#region Overrides of Context
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			base.DisposeResources(disposeManagedResources);
			if (!disposeManagedResources)
				return;
			if (repositoryDisposable != null)
				repositoryDisposable.Dispose();
		}
		#endregion
		#region Properties
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
		///<summary>
		/// Gets the HTTP context for this request.
		///</summary>
		public IHttpContext HttpContext
		{
			get
			{
				CheckDisposed();
				return httpContext;
			}
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
		private readonly IAutoPopStack<IControl> controlStack = new AutoPopStack<IControl>();
		private readonly IAutoPopStack<Form> formStack = new AutoPopStack<Form>();
		private readonly IHttpContext httpContext;
		private readonly IAutoPopStack<MailMessage> messageStack = new AutoPopStack<MailMessage>();
		private int controlIdGenerator;
		private IDisposable repositoryDisposable;
		#endregion
	}
}