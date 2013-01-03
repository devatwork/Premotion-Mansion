using System;
using System.IO;
using System.Reflection;
using System.Text;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Core.IO.EmbeddedResources
{
	/// <summary>
	/// Implements the <see cref="IResource"/> interface for embedded resources.
	/// </summary>
	public class EmbeddedResource : IResource
	{
		#region Nested type: EmbeddedResourceInputPipe
		/// <summary>
		/// Implements the <see cref="IInputPipe"/> for embedded resources.
		/// </summary>
		private class EmbeddedResourceInputPipe : DisposableBase, IInputPipe
		{
			#region Constructors
			/// <summary>
			/// Constructs the <see cref="EmbeddedResourceInputPipe"/>.
			/// </summary>
			/// <param name="resourceName">The name of the resource which to load.</param>
			/// <param name="assemblyName">The <see cref="Assembly"/> from which to load the resource.</param>
			public EmbeddedResourceInputPipe(string resourceName, Assembly assemblyName)
			{
				// validate arguments
				if (string.IsNullOrEmpty(resourceName))
					throw new ArgumentNullException("resourceName");
				if (assemblyName == null)
					throw new ArgumentNullException("assemblyName");

				// open the resource
				var resourceStream = assemblyName.GetManifestResourceStream(resourceName);
				if (resourceStream == null)
					throw new InvalidOperationException(string.Format("Could not load resource '{0}' from assembly '{1}'", resourceName, assemblyName.FullName));

				// open the resource
				reader = new StreamReader(resourceStream);
			}
			#endregion
			#region Implementation of IPipe
			/// <summary>
			/// Gets the encoding of this pipe.
			/// </summary>
			public Encoding Encoding
			{
				get { return Encoding.UTF8; }
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
		#region Constructors
		/// <summary>
		/// Constructs this embedded resource.
		/// </summary>
		/// <param name="name">The name of the resource.</param>
		/// <param name="assemblyName">The <see cref="Assembly"/> from which to load the resource.</param>
		/// <param name="path">The <see cref="IResourcePath"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if either <paramref name="name"/>, <paramref name="assemblyName"/> or <paramref name="path"/> is null.</exception>
		public EmbeddedResource(string name, AssemblyName assemblyName, IResourcePath path)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (assemblyName == null)
				throw new ArgumentNullException("assemblyName");
			if (path == null)
				throw new ArgumentNullException("path");

			// set values
			resourceName = assemblyName.Name + "." + name;
			this.assemblyName = assemblyName;
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
			return new EmbeddedResourceInputPipe(resourceName, Assembly.Load(assemblyName));
		}
		/// <summary>
		/// Opens this resource for writing.
		/// </summary>
		/// <returns>Returns a <see cref="IInputPipe"/>.</returns>
		public IOutputPipe OpenForWriting()
		{
			throw new NotSupportedException("Can not write to embedded resources.");
		}
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="IResource"/>.</returns>
		public string GetResourceIdentifier()
		{
			return resourceName;
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
			get { throw new NotSupportedException("The length is not supported on EmbeddedResources."); }
		}
		/// <summary>
		/// Gets the version of this resource.
		/// </summary>
		public string Version
		{
			get { return assemblyName.Version.ToString(); }
		}
		#endregion
		#region Private Fields
		private readonly AssemblyName assemblyName;
		private readonly string resourceName;
		#endregion
	}
}