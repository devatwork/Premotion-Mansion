using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Defines the public interface for responses.
	/// </summary>
	public interface IHttpResponse
	{
		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>The status code.</value>
		int StatusCode { get; set; }
		/// <summary>
		/// Gets or sets the HTTP status string of the output returned to the client.
		/// </summary>
		string StatusDescription { get; set; }
		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		string ContentType { get; set; }
		/// <summary>
		/// Gets or sets the HTTP character set of the output stream.
		/// </summary>
		Encoding ContentEncoding { get; set; }
		/// <summary>
		/// Gets or sets the HTTP character set of the output stream.
		/// </summary>
		string Charset { get; }
		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		Stream OutputStream { get; }
		/// <summary>
		/// Enables output of text to the outgoing HTTP response stream.
		/// </summary>
		/// <value>A <see cref="TextWriter"/> object that enables custom output to the client.</value>
		TextWriter Output { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is client connected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is client connected; otherwise, <c>false</c>.
		/// </value>
		bool IsClientConnected { get; }
		/// <summary>
		/// Gets the caching policy (such as expiration time, privacy settings, and vary clauses) of a Web page.
		/// </summary>
		HttpCachePolicy Cache { get; }
		/// <summary>
		/// Gets or sets the value of the Http Location header.
		/// </summary>
		string RedirectLocation { get; set; }
		/// <summary>
		/// Gets or sets the number of minutes before a page cached on a browser expires. If the user returns to the same page before it expires, the cached version is displayed. Expires is provided for compatibility with earlier versions of ASP.
		/// </summary>
		int Expires { get; set; }
		/// <summary>
		/// Gets the response cookie collection.
		/// </summary>
		HttpCookieCollection Cookies { get; }
		/// <summary>
		/// Gets the headers of this response.
		/// </summary>
		NameValueCollection Headers { get; }
		/// <summary>
		/// Gets or sets a value indicating whether to buffer output and send it after the complete response is finished processing.
		/// </summary>
		bool Buffer { get; set; }
		/// <summary>
		/// Appends the header.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		void AppendHeader(string name, string value);
		/// <summary>
		/// Writes the buffer to the browser
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		void BinaryWrite(byte[] buffer);
		/// <summary>
		/// Clears the response (only works if buffered)
		/// </summary>
		void Clear();
		/// <summary>
		/// Writes the specified string.
		/// </summary>
		/// <param name="s">The string.</param>
		void Write(string s);
		/// <summary>
		/// Writes the specified obj.
		/// </summary>
		/// <param name="obj">The obj.</param>
		void Write(object obj);
		/// <summary>
		/// Writes the file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		void WriteFile(string fileName);
		/// <summary>
		/// Creates a cookie.
		/// </summary>
		/// <param name="cookie">The cookie.</param>
		void AppendCookie(HttpCookie cookie);
		/// <summary>
		/// Updates an existing cookie in the cookie collection.
		/// </summary>
		/// <param name="cookie">The cookie in the collection to be updated.</param>
		void SetCookie(HttpCookie cookie);
		/// <summary>
		/// Ends the response.
		/// </summary>
		void End();
		/// <summary>
		/// Redirects a client to a new URL and specifies the new URL.
		/// </summary>
		/// <param name="url"></param>
		void Redirect(string url);
		/// <summary>
		/// Redirects a client to a new URL. Specifies the new URL and whether execution of the current page should terminate.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="endResponse"></param>
		void Redirect(string url, bool endResponse);
		/// <summary>
		/// Sends all currently buffered output to the client.
		/// </summary>
		void Flush();
		/// <summary>
		/// Adds a session ID to the virtual path if the session is using Cookieless session state and returns the combined path. If Cookieless session state is not used, ApplyAppPathModifier returns the original virtual path.
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		string ApplyAppPathModifier(string virtualPath);
		/// <summary>
		/// Adds an HTTP header to the output stream. AddHeader is provided for compatibility with earlier versions of ASP.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		void AddHeader(string name, string value);
	}
}