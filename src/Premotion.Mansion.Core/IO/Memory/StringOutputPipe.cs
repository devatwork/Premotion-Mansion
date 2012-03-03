using System;
using System.IO;
using System.Text;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Memory
{
	/// <summary>
	/// Implements <see cref="IOutputPipe"/> for strings.
	/// </summary>
	public class StringOutputPipe : DisposableBase, IOutputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="StringOutputPipe"/>.
		/// </summary>
		/// <param name="buffer">The buffer to which to write.</param>
		public StringOutputPipe(StringBuilder buffer)
		{
			// validate arguments
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			// open the writer
			writer = new StringWriter(buffer);
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
				return writer.Encoding;
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
				return writer;
			}
		}
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		public Stream RawStream
		{
			get { throw new NotSupportedException(); }
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

			// make sure the buffer is written
			writer.Flush();
			writer.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly StringWriter writer;
		#endregion
	}
}