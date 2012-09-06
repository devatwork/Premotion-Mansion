using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Web.Controls;
using Premotion.Mansion.Web.Controls.Forms;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Implements the <see cref="IMansionWebContext"/>.
	/// </summary>
	public class MansionWebContext : MansionContext, IMansionWebContext
	{
		#region Constants
		/// <summary>
		/// Key under which the <see cref="IMansionWebContext"/> is stored in the <see cref="HttpContextBase.Items"/> collection.
		/// </summary>
		private const string RequestContextKey = "mansion-request-context";
		#endregion
		#region Constructors
		private MansionWebContext(IMansionContext applicationContext, HttpContextBase httpContext, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(applicationContext.Nucleus)
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

			// create the application dataspace
			var baseUrl = ApplicationBaseUri.ToString().TrimEnd(Dispatcher.Constants.UrlPartTrimCharacters);
			var url = httpContext.Request.Url.ToString();
			Stack.Push("Request", new PropertyBag
			                      {
			                      	{"url", url},
			                      	{"urlPath", httpContext.Request.Url.GetLeftPart(UriPartial.Path)},
			                      	{"baseUrl", baseUrl}
			                      }, true);

			// set context location flag
			IsBackoffice = url.IndexOf("/" + Constants.BackofficeUrlPrefix + "/", baseUrl.Length, StringComparison.OrdinalIgnoreCase) == baseUrl.Length;

			// initialize the context
			IPropertyBag applicationSettings;
			if (!Stack.TryPeek(ApplicationSettingsConstants.DataspaceName, out applicationSettings))
				return;

			// try to get the culture
			string defaultCulture;
			if (applicationSettings.TryGet(applicationContext, "DEFAULT_SYSTEM_CULTURE", out defaultCulture) && !string.IsNullOrEmpty(defaultCulture))
				SystemCulture = CultureInfo.GetCultureInfo(defaultCulture);

			// try to get the culture
			string defaultUICulture;
			if (applicationSettings.TryGet(applicationContext, "DEFAULT_UI_CULTURE", out defaultUICulture) && !string.IsNullOrEmpty(defaultUICulture))
				UserInterfaceCulture = CultureInfo.GetCultureInfo(defaultUICulture);

			// initialize the repository, when possible
			var repositoryNamespace = applicationSettings.Get(this, ApplicationSettingsConstants.RepositoryNamespace, string.Empty);
			if (!string.IsNullOrEmpty(repositoryNamespace))
			{
				// open the repository
				repositoryDisposable = RepositoryUtil.Open(this, repositoryNamespace, applicationSettings);
			}
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
			var requestContext = new MansionWebContext(applicationContext, httpContext, applicationContext.Stack);

			// store the mansion request context in the http context
			httpContext.Items[RequestContextKey] = requestContext;

			// return teh context
			return requestContext;
		}
		/// <summary>
		/// Gets the Mansion request <see cref="IMansionWebContext"/> from <paramref name="httpContext"/>.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContextBase"/> from which to get the Mansion request context.</param>
		/// <returns>Returnss the Mansion request <see cref="IMansionWebContext"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="httpContext"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the <see cref="IMansionWebContext"/> was not found on the <paramref name="httpContext"/>.</exception>
		public static IMansionWebContext FetchFromHttpContext(HttpContextBase httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// get the request context
			var requestContext = httpContext.Items[RequestContextKey] as IMansionWebContext;
			if (requestContext == null)
				throw new InvalidOperationException("The request context was not found on the http context, please make sure the mansion http module is the first module");

			// return the context
			return requestContext;
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
		#region Overrides of MansionContext
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

			repositoryDisposable.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly Uri applicationBaseUri;
		private readonly IAutoPopStack<IControl> controlStack = new AutoPopStack<IControl>();
		private readonly IAutoPopStack<Form> formStack = new AutoPopStack<Form>();
		private readonly HttpContextBase httpContext;
		private readonly IAutoPopStack<MailMessage> messageStack = new AutoPopStack<MailMessage>();
		private readonly IDisposable repositoryDisposable;
		private int controlIdGenerator;
		#endregion
	}
}