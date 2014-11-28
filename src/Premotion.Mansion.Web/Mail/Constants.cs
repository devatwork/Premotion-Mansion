using System;
using System.Configuration;

namespace Premotion.Mansion.Web.Mail
{
	/// <summary>
	/// Defines constants for the web application mail library.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The namespace in which the mail tags live.
		/// </summary>
		public const string NamespaceUri = "http://schemas.premotion.nl/mansion/1.0/web/mail/tags.xsd";
		/// <summary>
		/// The name or IP address of the host used for SMTP transactions.
		/// </summary>
		public static string SmtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
		/// <summary>
		/// The port used for SMTP transactions.
		/// </summary>
		public static bool SmtpEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_ENABLE_SSL"]);
		/// <summary>
		/// Specify wether to use SSL to encrypt the SMTP connection.
		/// </summary>
		public static int SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
		/// <summary>
		/// The username for the SMTP network credentials.
		/// </summary>
		public static string SmtpUsername = ConfigurationManager.AppSettings["SMTP_USERNAME"];
		/// <summary>
		/// The password for the SMTP network credentials.
		/// </summary>
		public static string SmtpPassword = ConfigurationManager.AppSettings["SMTP_PASSWORD"];
		/// <summary>
		/// The staging e-mail address.
		/// </summary>
		public static string StagingEmail = ConfigurationManager.AppSettings["stagingEmail"];
	}
}