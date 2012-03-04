using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;

namespace Premotion.Mansion.Web.Mail.Standard
{
	/// <summary>
	/// Implements <see cref="IMailService"/> using the default ASP.NET <see cref="SmtpClient"/>.
	/// </summary>
	public class StandardMailService : ManagedLifecycleService, IMailService
	{
		#region Implementation of IMailService
		/// <summary>
		/// Sends the <paramref name="message"/> to the intended recepients.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="message">The <see cref="MailMessage"/> which to send.</param>
		public void Send(MansionContext context, MailMessage message)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (message == null)
				throw new ArgumentNullException("message");
			CheckDisposed();

			// if the application is not live, capture all mail
			var application = context.Stack.Peek<IPropertyBag>(ApplicationSettingsConstants.DataspaceName);
			var isApplicationStaging = !application.Get(context, ApplicationSettingsConstants.IsLiveApplicationSetting, false);
			if (isApplicationStaging)
			{
				var subject = new StringBuilder();
				subject.Append("[staging - to:");
				subject.Append(string.Join(";", message.To.Select(x => x.Address)));
				subject.Append(", CC:");
				subject.Append(string.Join(";", message.CC.Select(x => x.Address)));
				subject.Append(", BCC:");
				subject.Append(string.Join(";", message.Bcc.Select(x => x.Address)));
				subject.Append("] ");
				subject.Append(message.Subject);
				message.Subject = subject.ToString();
				message.To.Clear();
				message.CC.Clear();
				message.Bcc.Clear();

				// set the staging e-mail addresses
				string stagingEmailAddresses;
				if (!application.TryGet(context, "stagingEmail", out stagingEmailAddresses) || string.IsNullOrEmpty(stagingEmailAddresses))
					stagingEmailAddresses = "support@premotion.nl";
				message.To.Add(stagingEmailAddresses, string.Empty);
			}

			// send the message
			smptClient.Send(message);
		}
		/// <summary>
		/// Creates an <see cref="MailMessage"/>.
		/// </summary>
		/// <returns>Returns the created <see cref="MailMessage"/>.</returns>
		public MailMessage CreateMessage()
		{
			// just create an envelope
			return new MailMessage();
		}
		#endregion
		#region Overrides of ServiceBase
		/// <summary>
		/// Starts this service. All other services are initialized.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// create the client
			smptClient = new SmtpClient();
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
			// check for unmanaged disposal
			base.DisposeResources(disposeManagedResources);
			if (!disposeManagedResources)
				return;

			// dispose
			if (smptClient != null)
				smptClient.Dispose();
		}
		#endregion
		#region Private Fields
		private SmtpClient smptClient;
		#endregion
	}
}