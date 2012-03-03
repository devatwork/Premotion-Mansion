using System;
using System.Globalization;
using System.IO;
using System.Text;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.Windows
{
	/// <summary>
	/// Implements the <see cref="IResource"/> for windows files.
	/// </summary>
	public class FileResource : IResource
	{
		#region Nested type: FileInputPipe
		/// <summary>
		/// Implements <see cref="IInputPipe"/> for files.
		/// </summary>
		private class FileInputPipe : DisposableBase, IInputPipe
		{
			#region Constructors
			/// <summary>
			/// Constructs a <see cref="FileInputPipe"/>.
			/// </summary>
			/// <param name="fileInfo">The information on the file which to open.</param>
			public FileInputPipe(FileInfo fileInfo)
			{
				// validate arguments
				if (fileInfo == null)
					throw new ArgumentNullException("fileInfo");

				// make sure the directory exists
				if (fileInfo.Directory != null)
					fileInfo.Directory.Create();

				// open the reader
				reader = new StreamReader(fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read), Encoding.UTF8);
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
					return reader.CurrentEncoding;
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
					return reader.BaseStream;
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
			}
			#endregion
			#region Private Fields
			private readonly StreamReader reader;
			#endregion
		}
		#endregion
		#region Nested type: FileOutputPipe
		/// <summary>
		/// Implements <see cref="IOutputPipe"/> for files.
		/// </summary>
		private class FileOutputPipe : DisposableBase, IOutputPipe
		{
			#region Constructors
			/// <summary>
			/// Constructs a <see cref="FileOutputPipe"/>.
			/// </summary>
			/// <param name="fileInfo">The information on the file which to open.</param>
			public FileOutputPipe(FileInfo fileInfo)
			{
				// validate arguments
				if (fileInfo == null)
					throw new ArgumentNullException("fileInfo");

				// make sure the directory exists
				if (fileInfo.Directory != null)
					fileInfo.Directory.Create();

				// open the writer
				writer = new StreamWriter(fileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.None), Encoding.UTF8);
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
				get { return writer.BaseStream; }
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

				// makes sure all content is written
				writer.Flush();
				writer.Dispose();
			}
			#endregion
			#region Private Fields
			private readonly StreamWriter writer;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a resource.
		/// </summary>
		/// <param name="fileInfo"></param>
		/// <param name="path"></param>
		public FileResource(FileInfo fileInfo, IResourcePath path)
		{
			// validate arguments
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");
			if (path == null)
				throw new ArgumentNullException("path");

			// set values
			this.fileInfo = fileInfo;
			Path = path;
		}
		#endregion
		#region Implementation of IResource
		/// <summary>
		/// Opens this resource for reading.
		/// </summary>
		/// <returns>Returns a <see cref="IOutputPipe"/>.</returns>
		public IInputPipe OpenForReading()
		{
			return new FileInputPipe(fileInfo);
		}
		/// <summary>
		/// Opens this resource for writing.
		/// </summary>
		/// <returns>Returns a <see cref="IInputPipe"/>.</returns>
		public IOutputPipe OpenForWriting()
		{
			return new FileOutputPipe(fileInfo);
		}
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="IResource"/>.</returns>
		public string GetResourceIdentifier()
		{
			return fileInfo.FullName;
		}
		/// <summary>
		/// Gets the path of this resource.
		/// </summary>
		public IResourcePath Path { get; private set; }
		/// <summary>
		/// Gets the size of this resource in bytes.
		/// </summary>
		public long Length
		{
			get { return fileInfo.Length; }
		}
		/// <summary>
		/// Gets the version of this resource.
		/// </summary>
		public string Version
		{
			get { return fileInfo.LastWriteTime.Ticks.ToString(CultureInfo.InvariantCulture); }
		}
		#endregion
		#region Private Fields
		private readonly FileInfo fileInfo;
		#endregion
	}
}