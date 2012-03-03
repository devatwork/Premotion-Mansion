using System;
using System.IO;
using System.Text;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Memory
{
	/// <summary>
	/// Implements <see cref="IInputPipe"/> for string.
	/// </summary>
	public class StringInputPipe : DisposableBase, IInputPipe
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="StringInputPipe"/>.
		/// </summary>
		/// <param name="buffer">The buffer from which to read.</param>
		public StringInputPipe(StringBuilder buffer)
		{
			// validate arguments
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			// create the reader
			stream = new MemoryStream(Encoding.UTF8.GetBytes(buffer.ToString()));
			reader = new StreamReader(stream);
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
				return Encoding.UTF8;
			}
		}
		#endregion
		#region Implementation of IInputPipe
		/// <summary>
		/// Gets the reader for this pipe.
		/// </summary>
		public TextReader Reader
		{
			get
			{
				CheckDisposed();
				return reader;
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
				return stream;
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
			reader.Dispose();
			stream.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly StreamReader reader;
		private readonly MemoryStream stream;
		#endregion
	}
}