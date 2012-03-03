using System;
using System.Globalization;
using System.Text;

namespace Premotion.Mansion.Core.IO.Memory
{
	///<summary>
	/// Implements a literal resource which uses <see cref="string"/> as it's output.
	///</summary>
	public class LiteralResource : IResource
	{
		#region Constructors
		/// <summary>
		/// Constructs a literal resource.
		/// </summary>
		/// <param name="content">The content of this resource.</param>
		public LiteralResource(string content)
		{
			// validate arguments
			if (content == null)
				throw new ArgumentNullException("content");

			// set values
			buffer = new StringBuilder(content);
		}
		#endregion
		#region Implementation of IResource
		/// <summary>
		/// Opens this resource for reading.
		/// </summary>
		/// <returns>Returns a <see cref="IOutputPipe"/>.</returns>
		public IInputPipe OpenForReading()
		{
			return new StringInputPipe(buffer);
		}
		/// <summary>
		/// Opens this resource for writing.
		/// </summary>
		/// <returns>Returns a <see cref="IInputPipe"/>.</returns>
		public IOutputPipe OpenForWriting()
		{
			return new StringOutputPipe(buffer);
		}
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="IResource"/>.</returns>
		public string GetResourceIdentifier()
		{
			return buffer.ToString();
		}
		/// <summary>
		/// Gets the path of this resource.
		/// </summary>
		public IResourcePath Path
		{
			get { throw new NotSupportedException(); }
		}
		/// <summary>
		/// Gets the size of this resource in bytes.
		/// </summary>
		public long Length
		{
			get { return buffer.Length; }
		}
		/// <summary>
		/// Gets the version of this resource.
		/// </summary>
		public string Version
		{
			get { return buffer.Capacity.ToString(CultureInfo.InvariantCulture); }
		}
		#endregion
		#region Private Fields
		private readonly StringBuilder buffer;
		#endregion
	}
}