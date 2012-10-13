using System.Collections.Generic;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents a web request.
	/// </summary>
	public class WebRequest
	{
		/// <summary>
		/// Gets the headers.
		/// </summary>
		public IDictionary<string, string> Headers { get; private set; }
		/// <summary>
		/// Gets the <see cref="Url"/>.
		/// </summary>
		public Url Url { get; private set; }
		/// <summary>
		/// Gets the query string.
		/// </summary>
		public IDictionary<string, string> QueryString { get; private set; }
		/// <summary>
		/// Gets the user agent.
		/// </summary>
		public string UserAgent { get; set; }
		/// <summary>
		/// Gets the cookie collection.
		/// </summary>
		public IDictionary<string, WebCookie> Cookies { get; private set; }
		/// <summary>
		/// Gets the HTTP data transfer method used by the client.
		/// </summary>
		/// <value>The method.</value>
		public string Method { get; private set; }
		/// <summary>
		/// Gets a collection of files sent by the client-
		/// </summary>
		/// <value>An <see cref="IEnumerable{T}"/> instance, containing an <see cref="HttpFile"/> instance for each uploaded file.</value>
		public IDictionary<string, HttpFile> Files { get; private set; }
	}
}