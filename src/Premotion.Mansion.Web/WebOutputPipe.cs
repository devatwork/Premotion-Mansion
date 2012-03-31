using System;
using System.IO;
using System.Text;
using System.Web;
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
		/// <param name="httpContext">The responce stream.</param>
		public WebOutputPipe(HttpContextBase httpContext)
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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the content of the response in a byte array.</returns>
		public byte[] Flush(IMansionWebContext context)
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

			// return the content in bytes
			return contentBytes;
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
		private readonly HttpContextBase httpContext;
		#endregion
	}
}