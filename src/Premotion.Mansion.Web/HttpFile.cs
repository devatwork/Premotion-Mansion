using System.IO;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents a file that was captures in a HTTP multipart/form-data request
	/// </summary>
	public class HttpFile
	{
		#region Properties
		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>A <see cref="string"/> containing the content type of the file.</value>
		public string ContentType { get; private set; }
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>A <see cref="string"/> containing the name of the file.</value>
		public string FileName { get; private set; }
		/// <summary>
		/// Gets or sets the size of the file.
		/// </summary>
		/// <value>A <see cref="int"/> containing the size of the file.</value>
		public int ContentLength { get; private set; }
		/// <summary>
		/// Gets or sets the value stream.
		/// </summary>
		/// <value>A <see cref="Stream"/> containing the contents of the file.</value>
		public Stream InputStream { get; private set; }
		#endregion
	}
}