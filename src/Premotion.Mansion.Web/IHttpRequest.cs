using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Defines the public interface for requests.
	/// </summary>
	public interface IHttpRequest
	{
		/// <summary>
		/// Gets the raw URL.
		/// </summary>
		/// <value>The raw URL.</value>
		string RawUrl { get; }
		/// <summary>
		/// Gets the request URL.
		/// </summary>
		Uri Url { get; }
		/// <summary>
		/// Gets the referring URL.
		/// </summary>
		Uri UrlReferrer { get; }
		/// <summary>
		/// Gets the params which accumulates headers, post, querystring and cookies.
		/// </summary>
		/// <value>The params.</value>
		NameValueCollection Params { get; }
		/// <summary>
		/// Gets the query string.
		/// </summary>
		/// <value>The query string.</value>
		NameValueCollection QueryString { get; }
		/// <summary>
		/// Gets the form.
		/// </summary>
		/// <value>The form.</value>
		NameValueCollection Form { get; }
		/// <summary>
		/// Gets the server variables.
		/// </summary>
		/// <value>The server variables.</value>
		NameValueCollection ServerVariables { get; }
		/// <summary>
		/// Gets the Http headers.
		/// </summary>
		/// <value>The Http headers.</value>
		NameValueCollection Headers { get; }
		/// <summary>
		/// Indexer to access <see cref="Params"/> entries.
		/// </summary>
		/// <value></value>
		string this[string name] { get; }
		/// <summary>
		/// Gets the HTTP method.
		/// </summary>
		/// <value>The HTTP method.</value>
		string HttpMethod { get; }
		/// <summary>
		/// Gets the user agent string of the client browser.
		/// </summary>
		/// <value>The agent string of the client browser.</value>
		string UserAgent { get; }
		/// <summary>
		/// Gets the IP host address of the remote client. 
		/// </summary>
		/// <value>The IP address of the remote client.</value>
		string UserHostAddress { get; }
		/// <summary>
		/// Gets the contents of the incoming HTTP entity body.
		/// </summary>
		/// <value></value>
		Stream InputStream { get; }
		/// <summary>
		/// Gets the ASP.Net application's virtual application root path on the server.
		/// </summary>
		string ApplicationPath { get; }
		/// <summary>
		/// Gets the application <see cref="Uri"/>.
		/// </summary>
		Uri ApplicationBaseUri { get; }
		/// <summary>
		/// Gets/Sets the browser capabilities.
		/// </summary>
		HttpBrowserCapabilities Browser { get; }
		/// <summary>
		/// Gets the virtual path of the current request.
		/// </summary>
		string Path { get; }
		/// <summary>
		/// Gets additional path information for a resource with a URL extension.
		/// </summary>
		string PathInfo { get; }
		/// <summary>
		/// Gets a collection of cookies sent by the client.
		/// </summary>
		HttpCookieCollection Cookies { get; }
		/// <summary>
		/// Gets or sets the character set of the entity-body.
		/// </summary>
		Encoding ContentEncoding { get; }
		/// <summary>
		/// Gets the physical file system path corresponding to the requested URL.
		/// </summary>
		string PhysicalPath { get; }
		/// <summary>
		/// Gets the physical file system path of the currently executing server application's root directory.
		/// </summary>
		string PhysicalApplicationPath { get; }
		/// <summary>
		/// Gets the collection of files uploaded by the client, in multipart MIME format.
		/// </summary>
		HttpFileCollection Files { get; }
		/// <summary>
		/// Gets the accepting header encoding of this request.
		/// </summary>
		/// <returns>Returns 'gzip', 'deflate' or empty.</returns>
		string GetAcceptEncodingHeader();
	}
}