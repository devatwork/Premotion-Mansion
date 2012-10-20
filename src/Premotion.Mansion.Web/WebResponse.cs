using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Premotion.Mansion.Web
{
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
		public WebResponse Clone()
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
}