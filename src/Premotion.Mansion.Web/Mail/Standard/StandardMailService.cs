using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web.Mail.Standard
{
	/// <summary>
	/// Implements <see cref="IMailService"/> using the default ASP.NET <see cref="SmtpClient"/>.
	/// </summary>
	public class StandardMailService : DisposableBase, IMailService
	{
		#region Constructors
		/// <summary>
		/// Constructs a standard mail service.
		/// </summary>
		public StandardMailService()
		{
			// create the client
			smtpClient = new SmtpClient();

			if (!string.IsNullOrEmpty(Constants.SmtpHost))
				smtpClient.Host = Constants.SmtpHost;

			if (Constants.SmtpPort != 0)
				smtpClient.Port = Constants.SmtpPort;

			smtpClient.EnableSsl = Constants.SmtpEnableSsl;

			if (!String.IsNullOrEmpty(Constants.SmtpUsername) && !String.IsNullOrEmpty(Constants.SmtpPassword))
				smtpClient.Credentials = new System.Net.NetworkCredential(Constants.SmtpUsername, Constants.SmtpPassword);
		}
		#endregion
		#region Implementation of IMailService
		/// <summary>
		/// Sends the <paramref name="message"/> to the intended recepients.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="message">The <see cref="MailMessage"/> which to send.</param>
		public void Send(IMansionContext context, MailMessage message)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (message == null)
				throw new ArgumentNullException("message");
			CheckDisposed();

			// if the application is not live, capture all mail
			if (Core.Constants.ApplicationIsLive)
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
				var stagingEmailAddresses = Constants.StagingEmail;
				if (string.IsNullOrEmpty(stagingEmailAddresses))
					stagingEmailAddresses = "support@premotion.nl";
				message.To.Add(stagingEmailAddresses, string.Empty);
			}

			// send the message
			smtpClient.Send(message);
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
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// check for unmanaged disposal
			if (!disposeManagedResources)
				return;

			// dispose
			if (smtpClient != null)
				smtpClient.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly SmtpClient smtpClient;
		#endregion
	}
}