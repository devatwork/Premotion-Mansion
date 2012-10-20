using System;
using System.IO;
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
}