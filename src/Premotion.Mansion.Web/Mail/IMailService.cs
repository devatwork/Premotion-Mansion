﻿using System.Net.Mail;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Mail
{
	/// <summary>
	/// Represents the mail service.
	/// </summary>
	public interface IMailService
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="MailMessage"/>.
		/// </summary>
		/// <returns>Returns the created <see cref="MailMessage"/>.</returns>
		MailMessage CreateMessage();
		#endregion
		#region Send Message
		/// <summary>
		/// Sends the <paramref name="message"/> to the intended recepients.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="message">The <see cref="MailMessage"/> which to send.</param>
		void Send(IMansionContext context, MailMessage message);
		#endregion
	}
}