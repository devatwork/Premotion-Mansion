using System.Net.Mail;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Mail.ScriptTags
{
	/// <summary>
	/// Sends a e-mail message to the recipients.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "sendMail")]
	public class SendMailTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the values
			var recipients = GetRequiredAttribute<string>(context, "recipients");
			var recipientNames = GetAttribute(context, "recipientNames", string.Empty);
			var ccRecipients = GetAttribute(context, "ccRecipients", string.Empty);
			var ccRecipientNames = GetAttribute(context, "ccRecipientNames", string.Empty);
			var bccRecipients = GetAttribute(context, "bccRecipients", string.Empty);
			var bccRecipientNames = GetAttribute(context, "bccRecipientNames", string.Empty);
			var from = GetRequiredAttribute<string>(context, "from");
			var fromName = GetAttribute(context, "fromName", string.Empty);
			var sender = GetAttribute<string>(context, "sender");
			var senderName = GetAttribute(context, "senderName", string.Empty);
			var replyTos = GetAttribute(context, "replyTos", string.Empty);
			var replyToNames = GetAttribute(context, "replyToNames", string.Empty);
			var subject = GetRequiredAttribute<string>(context, "subject");

			// construct the message
			var mailService = context.Nucleus.ResolveSingle<IMailService>();
			using (var message = mailService.CreateMessage())
			{
				// set the properties
				message.To.Add(recipients, recipientNames);
				message.CC.Add(ccRecipients, ccRecipientNames);
				message.Bcc.Add(bccRecipients, bccRecipientNames);
				message.From = new MailAddress(from, fromName);
				if (!string.IsNullOrEmpty(sender))
					message.Sender = new MailAddress(sender, senderName);
				message.Subject = subject;
				message.ReplyToList.Add(replyTos, replyToNames);

				// allow childrern to modify the envelope
				using (context.Cast<IMansionWebContext>().MessageStack.Push(message))
					ExecuteChildTags(context);

				// send the mail
				mailService.Send(context, message);
			}
		}
		#endregion
	}
}