using System.IO;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents a file that was captures in a HTTP multipart/form-data request
	/// </summary>
	public class WebFile : DisposableBase
	{
		#region Properties
		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>A <see cref="string"/> containing the content type of the file.</value>
		public string ContentType { get; set; }
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>A <see cref="string"/> containing the name of the file.</value>
		public string FileName { get; set; }
		/// <summary>
		/// Gets or sets the size of the file.
		/// </summary>
		/// <value>A <see cref="int"/> containing the size of the file.</value>
		public int ContentLength { get; set; }
		/// <summary>
		/// Gets or sets the value stream.
		/// </summary>
		/// <value>A <see cref="Stream"/> containing the contents of the file.</value>
		public Stream InputStream { get; set; }
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

			if (InputStream != null)
				InputStream.Dispose();
		}
		#endregion
	}
}