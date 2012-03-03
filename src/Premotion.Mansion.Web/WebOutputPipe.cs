using System;
using System.Globalization;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns;
using Yahoo.Yui.Compressor;

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
		/// <param name="httpContext">The responce stream.</param>
		public WebOutputPipe(IHttpContext httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// set values
			this.httpContext = httpContext;

			bufferedWriter = new StreamWriter(buffer);
		}
		#endregion
		#region Implementation of IPipe
		/// <summary>
		/// Gets the encoding of this pipe.
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				CheckDisposed();
				return httpContext.Response.ContentEncoding;
			}
			set
			{
				CheckDisposed();
				httpContext.Response.ContentEncoding = value;
			}
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
		#region Cache Control Operations
		/// <summary>
		/// Gets/Set a flag indicating whether the output cache of this response is disabled.
		/// </summary>
		public bool OutputCacheEnabled { get; set; }
		/// <summary>
		/// Flushes this pipe to the response output.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <returns>Returns the content of the response in a byte array.</returns>
		public byte[] Flush(MansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			CheckDisposed();

			// get the content
			bufferedWriter.Flush();
			var contentBytes = new byte[buffer.Length];
			buffer.Position = 0;
			buffer.Read(contentBytes, 0, contentBytes.Length);

			// check if there is minification to do
			if (ContentType.Equals("text/css") || ContentType.Equals("application/x-javascript"))
			{
				// get the content string
				var content = Encoding.GetString(contentBytes);

				// minify css
				content = ContentType.Equals("text/css") ? CssCompressor.Compress(content) : JavaScriptCompressor.Compress(content, false, true, false, false, -1, Encoding, CultureInfo.InvariantCulture);

				// write back the bytes
				contentBytes = Encoding.GetBytes(content);
			}

			// compress the content when needed
			var compressedContentBytes = Compress(contentBytes);

			// return the content in bytes
			return compressedContentBytes;
		}
		#endregion
		#region Response Methods
		/// <summary>
		/// Appends an header to the response.
		/// </summary>
		/// <param name="name">The name of the header.</param>
		/// <param name="value">The value of the header.</param>
		public void AddHeader(string name, string value)
		{
			//  validate argugments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException("value");

			// set the header
			httpContext.Response.AddHeader(name, value);
		}
		#endregion
		#region Compress Methods
		/// <summary>
		/// Compresses the content before writing it to the client.
		/// </summary>
		/// <param name="contentBytes">The encoded bytes.</param>
		/// <returns>Returns the compressed bytes.</returns>
		private byte[] Compress(byte[] contentBytes)
		{
			// validate arguments
			if (contentBytes == null)
				throw new ArgumentNullException("contentBytes");

			// get the accept encoding from the request header
			var acceptEncoding = httpContext.Request.GetAcceptEncodingHeader();

			// check if browser accepts gzip
			if (HttpContextAdapter.GZip.Equals(acceptEncoding))
			{
				// set the header
				httpContext.Response.AddHeader("Content-encoding", HttpContextAdapter.GZip);

				// compress
				using (var bufferStream = new MemoryStream())
				{
					using (var compressorStream = new GZipOutputStream(bufferStream))
					{
						// set level to 1 indicating the fastest compression
						compressorStream.SetLevel(1);

						// write the content
						compressorStream.Write(contentBytes, 0, contentBytes.Length);
						compressorStream.Flush();
					}

					// return the compressed bytes
					return bufferStream.ToArray();
				}
			}

			// check if browser accepts deflate
			if (HttpContextAdapter.Deflate.Equals(acceptEncoding))
			{
				// set the header
				httpContext.Response.AddHeader("Content-encoding", HttpContextAdapter.Deflate);

				// compress
				using (var bufferStream = new MemoryStream())
				{
					using (var compressorStream = new DeflaterOutputStream(bufferStream, new Deflater(Deflater.BEST_SPEED, true)))
					{
						// write the content
						compressorStream.Write(contentBytes, 0, contentBytes.Length);
						compressorStream.Flush();
					}

					// return the compressed bytes
					return bufferStream.ToArray();
				}
			}

			// no compression method found, do not compress
			return contentBytes;
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

			// clean up
			bufferedWriter.Dispose();
			buffer.Dispose();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets/Sets the content type of the HTTP response.
		/// </summary>
		public string ContentType
		{
			get { return httpContext.Response.ContentType; }
			set
			{
				CheckDisposed();
				httpContext.Response.ContentType = value;
			}
		}
		#endregion
		#region Private Fields
		private readonly MemoryStream buffer = new MemoryStream();
		private readonly StreamWriter bufferedWriter;
		private readonly IHttpContext httpContext;
		#endregion
	}
}