using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Implements <see cref="IOutputPipe"/> for web applications.
	/// </summary>
	public class WebOutputPipe : DisposableBase, IOutputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs a web output pipe.
		/// </summary>
		/// <param name="response">The responce stream.</param>
		public WebOutputPipe(WebResponse response)
		{
			// validate arguments
			if (response == null)
				throw new ArgumentNullException("response");

			// set values
			this.response = response;

			bufferedWriter = new StreamWriter(buffer);
		}
		#endregion
		#region Implementation of IPipe
		/// <summary>
		/// Gets the encoding of this pipe.
		/// </summary>
		public Encoding Encoding
		{
			get { return response.ContentEncoding; }
			set { response.ContentEncoding = value; }
		}
		#endregion
		#region Implementation of IOutputPipe
		/// <summary>
		/// Gets the writer for this pipe.
		/// </summary>
		public TextWriter Writer
		{
			get
			{
				CheckDisposed();
				return bufferedWriter;
			}
		}
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		public Stream RawStream
		{
			get
			{
				CheckDisposed();
				return buffer;
			}
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
			if (!disposeManagedResources)
				return;

			// clean up the writer
			bufferedWriter.Flush();
			bufferedWriter.Dispose();

			// get the content bytes
			buffer.Flush();
			var contentBytes = buffer.ToArray();

			// set the contents
			response.Contents = stream => stream.Write(contentBytes, 0, contentBytes.Length);

			// clean up the buffer
			buffer.Dispose();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="WebResponse"/> of this pipe.
		/// </summary>
		public WebResponse Response
		{
			get { return response; }
		}
		#endregion
		#region Private Fields
		private readonly MemoryStream buffer = new MemoryStream();
		private readonly StreamWriter bufferedWriter;
		private readonly WebResponse response;
		#endregion
	}
	/// <summary>
	/// Represents a response.
	/// </summary>
	public class WebResponse
	{
		#region Constants
		/// <summary>
		/// Null object representing no body    
		/// </summary>
		private static readonly Action<Stream> NoBody = s => { };
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="WebResponse"/> class.
		/// </summary>
		protected WebResponse()
		{
			Contents = NoBody;
			ContentEncoding = Encoding.UTF8;
			ContentType = "text/html";
			Headers = new Dictionary<string, string>();
			StatusCode = HttpStatusCode.OK;
			Cookies = new List<WebCookie>(2);
			CacheSettings = new WebResponseSettings();
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static WebResponse Create(IMansionWebContext context)
		{
			return new WebResponse();
		}
		#endregion
		#region Clone Methods
		/// <summary>
		/// Clones this <see cref="WebResponse"/>.
		/// </summary>
		/// <returns>Returns the cloned instance.</returns>
		/// <remarks>Creates a deep copy, except for the <see cref="Contents"/>.</remarks>
		public virtual WebResponse Clone()
		{
			// get the buffer of the current web response
			byte[] contentBytes;
			using (var buffer = new MemoryStream())
			{
				// write out the current response
				Contents(buffer);

				// get the buffer
				contentBytes = buffer.ToArray();
			}

			// set the new contents
			Contents = stream => stream.Write(contentBytes, 0, contentBytes.Length);

			// create the clone
			var cloned = new WebResponse
			             {
			             	CacheSettings = CacheSettings.Clone(),
			             	ContentEncoding = ContentEncoding,
			             	Contents = stream => stream.Write(contentBytes, 0, contentBytes.Length),
			             	ContentType = ContentType,
			             	//Contents =  Contents,
			             	Cookies = new List<WebCookie>(Cookies),
			             	Headers = new Dictionary<string, string>(Headers),
			             	StatusCode = StatusCode,
			             	StatusDescription = StatusDescription,
			             	RedirectLocation = RedirectLocation
			             };

			// return the cloned object
			return cloned;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the <see cref="Encoding"/> of the content.
		/// </summary>
		/// <value>The <see cref="Encoding"/> of the content.</value>
		/// <remarks>The default value is <c>utf-8</c>.</remarks>
		public Encoding ContentEncoding { get; set; }
		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		/// <remarks>The default value is <c>text/html</c>.</remarks>
		public string ContentType { get; set; }
		/// <summary>
		/// Gets the delegate that will render contents to the response stream.
		/// </summary>
		/// <value>An <see cref="Action{T}"/> delegate, containing the code that will render contents to the response stream.</value>
		/// <remarks>The host of Nancy will pass in the output stream after the response has been handed back to it by Nancy.</remarks>
		public Action<Stream> Contents { get; set; }
		/// <summary>
		/// Gets the collection of HTTP response headers that should be sent back to the client.
		/// </summary>
		/// <value>An <see cref="IDictionary{TKey,TValue}"/> instance, contaning the key/value pair of headers.</value>
		public IDictionary<string, string> Headers { get; set; }
		/// <summary>
		/// Gets or sets the HTTP status code that should be sent back to the client.
		/// </summary>
		/// <value>A <see cref="HttpStatusCode"/> value.</value>
		public HttpStatusCode StatusCode { get; set; }
		/// <summary>
		/// Gets or sets the HTTP status description that should be sent back to the client.
		/// </summary>
		/// <value>A <see cref="HttpStatusCode"/> value.</value>
		public string StatusDescription { get; set; }
		/// <summary>
		/// Gets the <see cref="WebCookie"/> that should be sent back to the client.
		/// </summary>
		public IList<WebCookie> Cookies { get; protected set; }
		/// <summary>
		/// Gets the <see cref="WebResponseSettings"/> settings of this response.
		/// </summary>
		public WebResponseSettings CacheSettings { get; protected set; }
		/// <summary>
		/// Gets or sets the redirect location of the request if there is one.
		/// </summary>
		public string RedirectLocation { get; set; }
		#endregion
	}
	/// <summary>
	/// Defines the cache settings for a <see cref="WebResponse"/>
	/// </summary>
	public class WebResponseSettings
	{
		#region Constructors
		/// <summary></summary>
		public WebResponseSettings()
		{
			LastModified = DateTime.Now;
			OutputCacheEnabled = true;
		}
		#endregion
		#region Clone Methods
		/// <summary>
		/// Clones this <see cref="WebResponseSettings"/>.
		/// </summary>
		/// <returns>Returns the cloned instance.</returns>
		public WebResponseSettings Clone()
		{
			return new WebResponseSettings
			       {
			       	ETag = ETag,
			       	Expires = Expires,
			       	LastModified = LastModified,
			       	OutputCacheEnabled = OutputCacheEnabled
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the last modified date and time at which the cached information was creaded.
		/// </summary>
		public DateTime LastModified { get; set; }
		/// <summary>
		/// Gets or sets the absolute date and time at which cached information expires in the cache.
		/// </summary>
		public DateTime? Expires { get; set; }
		/// <summary>
		/// Gets/Set a flag indicating whether the output cache of this response is disabled.
		/// </summary>
		public bool OutputCacheEnabled { get; set; }
		/// <summary>
		/// Gets or sets the ETag of the current <see cref="WebResponse"/>.
		/// </summary>
		public string ETag { get; set; }
		#endregion
	}
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