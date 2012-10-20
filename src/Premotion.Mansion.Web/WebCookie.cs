using System;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents a web cookie.
	/// </summary>
	public class WebCookie
	{
		#region Properties
		/// <summary>
		/// The domain to restrict the cookie to
		/// </summary>
		public string Domain { get; set; }
		/// <summary>
		/// When the cookie should expire
		/// </summary>
		/// <value>A <see cref="DateTime"/> instance containing the date and time when the cookie should expire; otherwise <see langword="null"/> if it should never expire.</value>
		public DateTime? Expires { get; set; }
		/// <summary>
		/// Whether the cookie is http only
		/// </summary>
		public bool HttpOnly { get; set; }
		/// <summary>
		/// The name of the cookie
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Whether the cookie is secure (i.e. HTTPS only)
		/// </summary>
		public bool Secure { get; set; }
		/// <summary>
		/// The value of the cookie
		/// </summary>
		public string Value { get; set; }
		#endregion
	}
}