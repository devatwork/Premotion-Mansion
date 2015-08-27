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
		private const string MansionContextCacheKey = "__mansion_web_context__";
		#endregion
		#region Constructors
		private MansionWebContext(IMansionContext applicationContext, WebRequest request, IEnumerable<KeyValuePair<string, IEnumerable<IPropertyBag>>> initialStack) : base(applicationContext.Nucleus)
		{
			// validate arguments
			if (applicationContext == null)
				throw new ArgumentNullException("applicationContext");
			if (request == null)
				throw new ArgumentNullException("request");
			if (initialStack == null)
				throw new ArgumentNullException("initialStack");

			// set value
			Request = request;

			// create thet stack
			Stack = new AutoPopDictionaryStack<string, IPropertyBag>(StringComparer.OrdinalIgnoreCase, initialStack);

			// extracts needed dataspaces
			Stack.Push("Get", request.RequestUrl.QueryString.ToPropertyBag(), true);
			Stack.Push("Post", request.Form.ToPropertyBag(), true);
			Stack.Push("Headers", request.Headers.ToPropertyBag(), true);
			var requestUrlProperties = request.RequestUrl.ToPropertyBag();
			requestUrlProperties.Set("applicationUrl", request.ApplicationUrl);
			if (request.ReferrerUrl != null)
				requestUrlProperties.Set("referrerUrl", request.ReferrerUrl);
			Stack.Push("Request", requestUrlProperties, true);

			// set context location flag
			var backofficeRequest = request.RequestUrl.PathSegments.Length > 0 && request.RequestUrl.PathSegments[0].Equals(Constants.BackofficeUrlPrefix, StringComparison.OrdinalIgnoreCase);
			var backofficeReferrerRequest = request.ReferrerUrl != null && request.ReferrerUrl.PathSegments.Length > 0 && request.ReferrerUrl.PathSegments[0].Equals(Constants.BackofficeUrlPrefix, StringComparison.OrdinalIgnoreCase);

			IsBackoffice = (backofficeRequest || backofficeReferrerRequest);

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
			// open the repository
			repositoryDisposable = RepositoryUtil.Open(this);
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs an instance of <see cref="MansionWebContext"/>.
		/// </summary>
		/// <param name="applicationContext">The global application context.</param>
		/// <param name="request">The <see cref="HttpContextBase"/> of the current request.</param>
		/// <returns>Returns the constructed <see cref="IMansionWebContext"/>.</returns>
		public static MansionWebContext Create(IMansionContext applicationContext, WebRequest request)
		{
			// validate arguments
			if (applicationContext == null)
				throw new ArgumentNullException("applicationContext");
			if (request == null)
				throw new ArgumentNullException("request");

			// use the 
			if (request.Cache.Contains(MansionContextCacheKey))
				return (MansionWebContext) request.Cache[MansionContextCacheKey];

			// create the context
			var context = new MansionWebContext(applicationContext, request, applicationContext.Stack);

			// store the context in the cache
			request.Cache.Add(MansionContextCacheKey, context);

			// return the context
			return context;
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
		/// Gets the <see cref="WebRequest"/>.
		/// </summary>
		public WebRequest Request { get; private set; }
		/// <summary>
		/// Gets the <see cref="ISession"/>.
		/// </summary>
		public ISession Session { get; set; }
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
			Request.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IAutoPopStack<IControl> controlStack = new AutoPopStack<IControl>();
		private readonly IAutoPopStack<Form> formStack = new AutoPopStack<Form>();
		private readonly IAutoPopStack<MailMessage> messageStack = new AutoPopStack<MailMessage>();
		private readonly IDisposable repositoryDisposable;
		private int controlIdGenerator;
		#endregion
	}
}