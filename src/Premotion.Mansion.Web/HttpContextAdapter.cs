using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Implements the adapter for the HTTP context.
	/// </summary>
	public class HttpContextAdapter : IHttpContext
	{
		#region Constants
		private const string Identity = @"identity";
		/// <summary>
		/// Defines the GZip constant.
		/// </summary>
		public const string GZip = @"gzip";
		/// <summary>
		/// Defines the Deflate constant.
		/// </summary>
		public const string Deflate = @"deflate";
		private static readonly string[] acceptEncodingHeaders = new[] {Identity, GZip, Deflate};
		#endregion
		#region Nested type: HttpRequestAdapter
		/// <summary>
		/// Implements the adapter for the HTTP requests.
		/// </summary>
		private class HttpRequestAdapter : IHttpRequest
		{
			#region Nested type: QValue
			/// <summary>
			/// Represens a weighted encoding value.
			/// </summary>
			public class QValue
			{
				#region Constants
				private static readonly char[] separators = new[] {'=', ';'};
				#endregion
				#region Constructors
				/// <summary>
				/// Use <see cref="Parse"/>.
				/// </summary>
				private QValue()
				{
				}
				#endregion
				#region Factory Methods
				/// <summary>
				/// Parses <paramref name="value"/> into an <see cref="QValue"/>.
				/// </summary>
				/// <param name="value">The value which to parse, gzip;q=0.5</param>
				/// <returns>Returns the parse <see cref="QValue"/>.</returns>
				public static QValue Parse(string value)
				{
					// validate arguments
					if (string.IsNullOrEmpty(value))
						throw new ArgumentNullException("value");

					// split the value
					var valueParts = value.Split(separators);
					float weight;
					if (valueParts.Length != 3 || float.TryParse(valueParts[2], out weight))
						weight = 1;

					// create the value
					return new QValue
					       {
					       	Name = valueParts[0],
					       	Weight = weight
					       };
				}
				#endregion
				#region Properties
				/// <summary>
				/// Gets the weight of this value.
				/// Gets the weight of this value.
				/// </summary>
				public float Weight { get; private set; }
				/// <summary>
				/// Gets the name of this value.
				/// </summary>
				public string Name { get; private set; }
				#endregion
			}
			#endregion
			#region Nested type: QValueList
			/// <summary>
			/// Provides a collection for working with <see cref="QValue"/> http headers
			/// </summary>
			/// <remarks>
			/// accept-encoding spec:
			///    http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
			/// </remarks>
			private class QValueList
			{
				#region Constants
				private static readonly char[] separators = new[] {','};
				#endregion
				#region Constructors
				/// <summary>
				/// Constructs a list of <see cref="QValue"/>s.
				/// </summary>
				/// <param name="values">The values.</param>
				private QValueList(IEnumerable<QValue> values)
				{
					// validate arguments
					if (values == null)
						throw new ArgumentNullException("values");

					// set values
					this.values = values.ToArray();
					AcceptWildcard = this.values.Any(candidate => "*".Equals(candidate.Name, StringComparison.InvariantCultureIgnoreCase));
				}
				#endregion
				#region  Factory Methods
				/// <summary>
				/// Parses <paramref name="value"/> into a <see cref="QValueList"/>.
				/// </summary>
				/// <param name="value"></param>
				/// <returns></returns>
				public static QValueList Parse(string value)
				{
					// parse
					return new QValueList((value ?? string.Empty).Split(separators).Select(QValue.Parse));
				}
				#endregion
				#region Properties
				/// <summary>
				/// Gets a flag indicating whether wildcard encoding is accepted.
				/// </summary>
				public bool AcceptWildcard { get; private set; }
				/// <summary>
				/// Gets the <see cref="QValue"/>s in this list.
				/// </summary>
				public IEnumerable<QValue> Values
				{
					get { return values; }
				}
				#endregion
				#region Private Fields
				private readonly IEnumerable<QValue> values;
				#endregion
			}
			#endregion
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="request"></param>
			public HttpRequestAdapter(HttpRequest request)
			{
				// validate arguments
				if (request == null)
					throw new ArgumentNullException("request");

				// create the application uri
				var applicationUriBuilder = new UriBuilder(request.Url)
				                            {
				                            	Fragment = string.Empty,
				                            	Query = string.Empty,
				                            	Path = (request.ApplicationPath ?? string.Empty).Trim('/') + "/"
				                            };

				// set values
				this.request = request;
				applicationBaseUri = applicationUriBuilder.Uri;
			}
			#endregion
			#region Implementation of IRequest
			/// <summary>
			/// Gets the collection of files uploaded by the client, in multipart MIME format.
			/// </summary>
			public HttpFileCollection Files
			{
				get { return request.Files; }
			}
			/// <summary>
			/// Gets the raw URL.
			/// </summary>
			/// <value>The raw URL.</value>
			public string RawUrl
			{
				get { return request.RawUrl; }
			}
			/// <summary>
			/// Gets the request URL.
			/// </summary>
			public Uri Url
			{
				get { return request.Url; }
			}
			/// <summary>
			/// Gets the referring URL.
			/// </summary>
			public Uri UrlReferrer
			{
				get { return request.UrlReferrer; }
			}
			/// <summary>
			/// Gets the params which accumulates headers, post, querystring and cookies.
			/// </summary>
			/// <value>The params.</value>
			public NameValueCollection Params
			{
				get { return request.Params; }
			}
			/// <summary>
			/// Gets the query string.
			/// </summary>
			/// <value>The query string.</value>
			public NameValueCollection QueryString
			{
				get { return request.QueryString; }
			}
			/// <summary>
			/// Gets the form.
			/// </summary>
			/// <value>The form.</value>
			public NameValueCollection Form
			{
				get { return request.Form; }
			}
			/// <summary>
			/// Gets the server variables.
			/// </summary>
			/// <value>The server variables.</value>
			public NameValueCollection ServerVariables
			{
				get { return request.ServerVariables; }
			}
			/// <summary>
			/// Gets the Http headers.
			/// </summary>
			/// <value>The Http headers.</value>
			public NameValueCollection Headers
			{
				get { return request.Headers; }
			}
			/// <summary>
			/// Indexer to access <see cref="IHttpRequest.Params"/> entries.
			/// </summary>
			/// <value></value>
			public string this[string name]
			{
				get { return request[name]; }
			}
			/// <summary>
			/// Gets the HTTP method.
			/// </summary>
			/// <value>The HTTP method.</value>
			public string HttpMethod
			{
				get { return request.HttpMethod; }
			}
			/// <summary>
			/// Gets the user agent string of the client browser.
			/// </summary>
			/// <value>The agent string of the client browser.</value>
			public string UserAgent
			{
				get { return request.UserAgent; }
			}
			/// <summary>
			/// Gets the IP host address of the remote client. 
			/// </summary>
			/// <value>The IP address of the remote client.</value>
			public string UserHostAddress
			{
				get { return request.UserHostAddress; }
			}
			/// <summary>
			/// Gets the contents of the incoming HTTP entity body.
			/// </summary>
			/// <value></value>
			public Stream InputStream
			{
				get { return request.InputStream; }
			}
			/// <summary>
			/// Gets the ASP.Net application's virtual application root path on the server.
			/// </summary>
			public string ApplicationPath
			{
				get { return request.ApplicationPath; }
			}
			/// <summary>
			/// Gets the application <see cref="Uri"/>.
			/// </summary>
			public Uri ApplicationBaseUri
			{
				get { return applicationBaseUri; }
			}
			/// <summary>
			/// Gets/Sets the browser capabilities.
			/// </summary>
			public HttpBrowserCapabilities Browser
			{
				get { return request.Browser; }
			}
			/// <summary>
			/// Gets the virtual path of the current request.
			/// </summary>
			public string Path
			{
				get { return request.Path; }
			}
			/// <summary>
			/// Gets additional path information for a resource with a URL extension.
			/// </summary>
			public string PathInfo
			{
				get { return request.PathInfo; }
			}
			/// <summary>
			/// Gets a collection of cookies sent by the client.
			/// </summary>
			public HttpCookieCollection Cookies
			{
				get { return request.Cookies; }
			}
			/// <summary>
			/// Gets or sets the character set of the entity-body.
			/// </summary>
			public Encoding ContentEncoding
			{
				get { return request.ContentEncoding; }
			}
			/// <summary>
			/// Gets the physical file system path corresponding to the requested URL.
			/// </summary>
			public string PhysicalPath
			{
				get { return request.PhysicalPath; }
			}
			/// <summary>
			/// Gets the physical file system path of the currently executing server application's root directory.
			/// </summary>
			public string PhysicalApplicationPath
			{
				get { return request.PhysicalApplicationPath; }
			}
			/// <summary>
			/// Gets the accepting header encoding of this request.
			/// </summary>
			/// <returns>Returns 'gzip', 'deflate' or empty.</returns>
			public string GetAcceptEncodingHeader()
			{
				// load encodings from header
				var encodings = QValueList.Parse(Headers["Accept-Encoding"]);

				// get the types we can handle, can be accepted and in the defined client preference
				var preferred = encodings.Values.Where(x => acceptEncodingHeaders.Contains(x.Name)).OrderBy(x => x.Weight).FirstOrDefault();

				// if none of the preferred values were found, but the client can accept wildcard encodings, we'll default to Gzip.
				if (preferred == null && encodings.AcceptWildcard)
					return GZip;
				if (preferred == null)
					return string.Empty;

				// handle the preferred encoding
				switch (preferred.Name)
				{
					case GZip:
					case Deflate:
						return preferred.Name;
					default:
						return string.Empty;
				}
			}
			#endregion
			#region Private Fields
			private readonly Uri applicationBaseUri;
			private readonly HttpRequest request;
			#endregion
		}
		#endregion
		#region Nested type: HttpResponseAdapter
		/// <summary>
		/// Implements the adapter for the HTTP responses.
		/// </summary>
		private class HttpResponseAdapter : IHttpResponse
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="response"></param>
			public HttpResponseAdapter(HttpResponse response)
			{
				// validate arguments
				if (response == null)
					throw new ArgumentNullException("response");

				// set values
				this.response = response;
			}
			#endregion
			#region Implementation of IHttpResponse
			/// <summary>
			/// Gets the headers of this response.
			/// </summary>
			public NameValueCollection Headers
			{
				get { return response.Headers; }
			}
			/// <summary>
			/// Gets or sets a value indicating whether to buffer output and send it after the complete response is finished processing.
			/// </summary>
			public bool Buffer
			{
				get { return response.Buffer; }
				set { response.Buffer = value; }
			}
			/// <summary>
			/// Gets or sets the status code.
			/// </summary>
			/// <value>The status code.</value>
			public int StatusCode
			{
				get { return response.StatusCode; }
				set { response.StatusCode = value; }
			}
			/// <summary>
			/// Gets or sets the HTTP status string of the output returned to the client.
			/// </summary>
			public string StatusDescription
			{
				get { return response.StatusDescription; }
				set { response.StatusDescription = value; }
			}
			/// <summary>
			/// Gets or sets the type of the content.
			/// </summary>
			/// <value>The type of the content.</value>
			public string ContentType
			{
				get { return response.ContentType; }
				set { response.ContentType = value; }
			}
			/// <summary>
			/// Gets or sets the HTTP character set of the output stream.
			/// </summary>
			public Encoding ContentEncoding
			{
				get { return response.ContentEncoding; }
				set { response.ContentEncoding = value; }
			}
			/// <summary>
			/// Gets or sets the HTTP character set of the output stream.
			/// </summary>
			public string Charset
			{
				get { return response.Charset; }
			}
			/// <summary>
			/// Enables output of text to the outgoing HTTP response stream.
			/// </summary>
			/// <value>A <see cref="TextWriter"/> object that enables custom output to the client.</value>
			public TextWriter Output
			{
				get { return response.Output; }
			}
			/// <summary>
			/// Gets the output stream.
			/// </summary>
			/// <value>The output stream.</value>
			public Stream OutputStream
			{
				get { return response.OutputStream; }
			}
			/// <summary>
			/// Gets a value indicating whether this instance is client connected.
			/// </summary>
			/// <value>
			/// 	<c>true</c> if this instance is client connected; otherwise, <c>false</c>.
			/// </value>
			public bool IsClientConnected
			{
				get { return response.IsClientConnected; }
			}
			/// <summary>
			/// Gets the caching policy (such as expiration time, privacy settings, and vary clauses) of a Web page.
			/// </summary>
			public HttpCachePolicy Cache
			{
				get { return response.Cache; }
			}
			/// <summary>
			/// Gets or sets the value of the Http Location header.
			/// </summary>
			public string RedirectLocation
			{
				get { return response.RedirectLocation; }
				set { response.RedirectLocation = value; }
			}
			/// <summary>
			/// Gets or sets the number of minutes before a page cached on a browser expires. If the user returns to the same page before it expires, the cached version is displayed. Expires is provided for compatibility with earlier versions of ASP.
			/// </summary>
			public int Expires
			{
				get { return response.Expires; }
				set { response.Expires = value; }
			}
			/// <summary>
			/// Appends the header.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public void AppendHeader(string name, string value)
			{
				response.AppendHeader(name, value);
			}
			/// <summary>
			/// Writes the buffer to the browser
			/// </summary>
			/// <param name="buffer">The buffer.</param>
			public void BinaryWrite(byte[] buffer)
			{
				response.BinaryWrite(buffer);
			}
			/// <summary>
			/// Clears the response (only works if buffered)
			/// </summary>
			public void Clear()
			{
				response.Clear();
			}
			/// <summary>
			/// Writes the specified string.
			/// </summary>
			/// <param name="s">The string.</param>
			public void Write(string s)
			{
				response.Write(s);
			}
			/// <summary>
			/// Writes the specified obj.
			/// </summary>
			/// <param name="obj">The obj.</param>
			public void Write(object obj)
			{
				response.Write(obj);
			}
			/// <summary>
			/// Writes the file.
			/// </summary>
			/// <param name="fileName">Name of the file.</param>
			public void WriteFile(string fileName)
			{
				response.WriteFile(fileName);
			}
			/// <summary>
			/// Creates a cookie.
			/// </summary>
			/// <param name="cookie">The cookie.</param>
			public void AppendCookie(HttpCookie cookie)
			{
				response.AppendCookie(cookie);
			}
			/// <summary>
			/// Updates an existing cookie in the cookie collection.
			/// </summary>
			/// <param name="cookie">The cookie in the collection to be updated.</param>
			public void SetCookie(HttpCookie cookie)
			{
				response.SetCookie(cookie);
			}
			/// <summary>
			/// Ends the response.
			/// </summary>
			public void End()
			{
				response.End();
			}
			/// <summary>
			/// Redirects a client to a new URL and specifies the new URL.
			/// </summary>
			/// <param name="url"></param>
			public void Redirect(string url)
			{
				response.Redirect(url);
			}
			/// <summary>
			/// Redirects a client to a new URL. Specifies the new URL and whether execution of the current page should terminate.
			/// </summary>
			/// <param name="url"></param>
			/// <param name="endResponse"></param>
			public void Redirect(string url, bool endResponse)
			{
				response.Redirect(url, endResponse);
			}
			/// <summary>
			/// Sends all currently buffered output to the client.
			/// </summary>
			public void Flush()
			{
				response.Flush();
			}
			/// <summary>
			/// Adds a session ID to the virtual path if the session is using Cookieless session state and returns the combined path. If Cookieless session state is not used, ApplyAppPathModifier returns the original virtual path.
			/// </summary>
			/// <param name="virtualPath"></param>
			/// <returns></returns>
			public string ApplyAppPathModifier(string virtualPath)
			{
				return response.ApplyAppPathModifier(virtualPath);
			}
			/// <summary>
			/// Adds an HTTP header to the output stream. AddHeader is provided for compatibility with earlier versions of ASP.
			/// </summary>
			/// <param name="name"></param>
			/// <param name="value"></param>
			public void AddHeader(string name, string value)
			{
				response.AddHeader(name, value);
			}
			/// <summary>
			/// Gets the response cookie collection.
			/// </summary>
			public HttpCookieCollection Cookies
			{
				get { return response.Cookies; }
			}
			#endregion
			#region Private Fields
			private readonly HttpResponse response;
			#endregion
		}
		#endregion
		#region Nested type: HttpSessionStateAdapter
		/// <summary>
		/// Wraps the session state adapter.
		/// </summary>
		private class HttpSessionStateAdapter : IHttpSessionState
		{
			#region Constructors
			/// <summary>
			/// Constructs this adapter.
			/// </summary>
			/// <param name="state">The see <see cref="HttpSessionState"/>.</param>
			public HttpSessionStateAdapter(HttpSessionState state)
			{
				// validate arguments
				if (state == null)
					throw new ArgumentNullException("state");

				// set values
				this.state = state;
			}
			#endregion
			#region Implementation of IHttpSessionState
			/// <summary>
			/// Ends the current session.
			/// </summary>
			public void Abandon()
			{
				state.Abandon();
			}
			/// <summary>
			/// Adds a new item to the session-state collection.
			/// </summary>
			/// <param name="name">The name of the item to add to the session-state collection. 
			///                 </param><param name="value">The value of the item to add to the session-state collection. 
			///                 </param>
			public void Add(string name, object value)
			{
				state.Add(name, value);
			}
			/// <summary>
			/// Deletes an item from the session-state item collection.
			/// </summary>
			/// <param name="name">The name of the item to delete from the session-state item collection. 
			///                 </param>
			public void Remove(string name)
			{
				state.Remove(name);
			}
			/// <summary>
			/// Deletes an item at a specified index from the session-state item collection.
			/// </summary>
			/// <param name="index">The index of the item to remove from the session-state collection. 
			///                 </param>
			public void RemoveAt(int index)
			{
				state.RemoveAt(index);
			}
			/// <summary>
			/// Clears all values from the session-state item collection.
			/// </summary>
			public void Clear()
			{
				state.Clear();
			}
			/// <summary>
			/// Clears all values from the session-state item collection.
			/// </summary>
			public void RemoveAll()
			{
				state.RemoveAll();
			}
			/// <summary>
			/// Returns an enumerator that can be used to read all the session-state item values in the current session.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Collections.IEnumerator"/> that can iterate through the values in the session-state item collection.
			/// </returns>
			public IEnumerator GetEnumerator()
			{
				return state.GetEnumerator();
			}
			/// <summary>
			/// Copies the collection of session-state item values to a one-dimensional array, starting at the specified index in the array.
			/// </summary>
			/// <param name="array">The <see cref="T:System.Array"/> that receives the session values. 
			///                 </param><param name="index">The index in <paramref name="array"/> where copying starts. 
			///                 </param>
			public void CopyTo(Array array, int index)
			{
				state.CopyTo(array, index);
			}
			/// <summary>
			/// Gets the unique session identifier for the session.
			/// </summary>
			/// <returns>
			/// The session ID.
			/// </returns>
			public string SessionID
			{
				get { return state.SessionID; }
			}
			/// <summary>
			/// Gets and sets the time-out period (in minutes) allowed between requests before the session-state provider terminates the session.
			/// </summary>
			/// <returns>
			/// The time-out period, in minutes.
			/// </returns>
			public int Timeout
			{
				get { return state.Timeout; }
				set { state.Timeout = value; }
			}
			/// <summary>
			/// Gets a value indicating whether the session was created with the current request.
			/// </summary>
			/// <returns>
			/// true if the session was created with the current request; otherwise, false.
			/// </returns>
			public bool IsNewSession
			{
				get { return state.IsNewSession; }
			}
			/// <summary>
			/// Gets the current session-state mode.
			/// </summary>
			/// <returns>
			/// One of the <see cref="T:System.Web.SessionState.SessionStateMode"/> values.
			/// </returns>
			public SessionStateMode Mode
			{
				get { return state.Mode; }
			}
			/// <summary>
			/// Gets a value indicating whether the session ID is embedded in the URL or stored in an HTTP cookie.
			/// </summary>
			/// <returns>
			/// true if the session is embedded in the URL; otherwise, false.
			/// </returns>
			public bool IsCookieless
			{
				get { return state.IsCookieless; }
			}
			/// <summary>
			/// Gets a value that indicates whether the application is configured for cookieless sessions.
			/// </summary>
			/// <returns>
			/// One of the <see cref="T:System.Web.HttpCookieMode"/> values that indicate whether the application is configured for cookieless sessions. The default is <see cref="F:System.Web.HttpCookieMode.UseCookies"/>.
			/// </returns>
			public HttpCookieMode CookieMode
			{
				get { return state.CookieMode; }
			}
			/// <summary>
			/// Gets or sets the locale identifier (LCID) of the current session.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.Globalization.CultureInfo"/> instance that specifies the culture of the current session.
			/// </returns>
			public int LCID
			{
				get { return state.LCID; }
				set { state.LCID = value; }
			}
			/// <summary>
			/// Gets or sets the code-page identifier for the current session.
			/// </summary>
			/// <returns>
			/// The code-page identifier for the current session.
			/// </returns>
			public int CodePage
			{
				get { return state.CodePage; }
				set { state.CodePage = value; }
			}
			/// <summary>
			/// Gets a collection of objects declared by &lt;object Runat="Server" Scope="Session"/&gt; tags within the ASP.NET application file Global.asax.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Web.HttpStaticObjectsCollection"/> containing objects declared in the Global.asax file.
			/// </returns>
			public HttpStaticObjectsCollection StaticObjects
			{
				get { return state.StaticObjects; }
			}
			/// <summary>
			/// Gets or sets a session-state item value by name.
			/// </summary>
			/// <returns>
			/// The session-state item value specified in the <paramref name="name"/> parameter.
			/// </returns>
			/// <param name="name">The key name of the session-state item value. 
			///                 </param>
			object IHttpSessionState.this[string name]
			{
				get { return state[name]; }
				set { state[name] = value; }
			}
			/// <summary>
			/// Gets or sets a session-state item value by numerical index.
			/// </summary>
			/// <returns>
			/// The session-state item value specified in the <paramref name="index"/> parameter.
			/// </returns>
			/// <param name="index">The numerical index of the session-state item value. 
			///                 </param>
			object IHttpSessionState.this[int index]
			{
				get { return state[index]; }
				set { state[index] = value; }
			}
			/// <summary>
			/// Gets the number of items in the session-state item collection.
			/// </summary>
			/// <returns>
			/// The number of items in the session-state item collection.
			/// </returns>
			public int Count
			{
				get { return state.Count; }
			}
			/// <summary>
			/// Gets a collection of the keys for all values stored in the session-state item collection.
			/// </summary>
			/// <returns>
			/// The <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection"/> that contains all the session-item keys.
			/// </returns>
			public NameObjectCollectionBase.KeysCollection Keys
			{
				get { return state.Keys; }
			}
			/// <summary>
			/// Gets an object that can be used to synchronize access to the collection of session-state values.
			/// </summary>
			/// <returns>
			/// An object that can be used to synchronize access to the collection.
			/// </returns>
			public object SyncRoot
			{
				get { return state.SyncRoot; }
			}
			/// <summary>
			/// Gets a value indicating whether the session is read-only.
			/// </summary>
			/// <returns>
			/// true if the session is read-only; otherwise, false.
			/// </returns>
			public bool IsReadOnly
			{
				get { return state.IsReadOnly; }
			}
			/// <summary>
			/// Gets a value indicating whether access to the collection of session-state values is synchronized (thread safe).
			/// </summary>
			/// <returns>
			/// true if access to the collection is synchronized (thread safe); otherwise, false.
			/// </returns>
			public bool IsSynchronized
			{
				get { return state.IsSynchronized; }
			}
			#endregion
			#region Private Fields
			private readonly HttpSessionState state;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		private HttpContextAdapter(HttpContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// set values
			this.context = context;
			request = new HttpRequestAdapter(context.Request);
			response = new HttpResponseAdapter(context.Response);
			session = context.Session != null ? new HttpSessionStateAdapter(context.Session) : null;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Adapts the <paramref name="httpContext"/> to <see cref="IHttpContext"/>.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContext"/> which to adapt.</param>
		/// <returns>Returns <see cref="IHttpContext"/>.</returns>
		public static IHttpContext Adapt(HttpContext httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			return new HttpContextAdapter(httpContext);
		}
		#endregion
		#region Implementation of IHttpContext
		/// <summary>
		/// Gets the request object.
		/// </summary>
		public IHttpRequest Request
		{
			get { return request; }
		}
		/// <summary>
		/// Gets the response object.
		/// </summary>
		public IHttpResponse Response
		{
			get { return response; }
		}
		/// <summary>
		/// Gets the Cache object for the current application domain.
		/// </summary>
		public Cache Cache
		{
			get { return context.Cache; }
		}
		/// <summary>
		/// Gets the HttpServerUtility object that provides methods used in processing Web requests.
		/// </summary>
		public HttpServerUtility Server
		{
			get { return context.Server; }
		}
		/// <summary>
		/// Gets a flag indicating whether there is a session for this request.
		/// </summary>
		public bool HasSession
		{
			get { return session != null; }
		}
		/// <summary>
		/// Gets the HttpSessionState object for the current HTTP request.
		/// </summary>
		public IHttpSessionState Session
		{
			get
			{
				if (session == null)
					throw new ApplicationException("There is no session in this request.");

				return session;
			}
		}
		/// <summary>
		/// Gets or sets the HttpApplication object for the current HTTP request.
		/// </summary>
		public HttpApplication ApplicationInstance
		{
			get { return context.ApplicationInstance; }
		}
		/// <summary>
		/// Gets the TraceContext object for the current HTTP response.
		/// </summary>
		public TraceContext Trace
		{
			get { return context.Trace; }
		}
		/// <summary>
		/// Gets a key/value collection that can be used to organize and share data between an <see cref="IHttpModule"/> interface and an <see cref="IHttpHandler"/> interface during an HTTP request.
		/// </summary>
		public IDictionary Items
		{
			get { return context.Items; }
		}
		/// <summary>
		/// Rewrites the URL using the given path.
		/// </summary>
		/// <param name="path">The internal rewrite path.</param>
		public void RewritePath(string path)
		{
			context.RewritePath(path);
		}
		#endregion
		#region Private Fields
		private readonly HttpContext context;
		private readonly IHttpRequest request;
		private readonly IHttpResponse response;
		private readonly IHttpSessionState session;
		#endregion
	}
}