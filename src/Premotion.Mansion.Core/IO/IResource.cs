namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a resource.
	/// </summary>
	public interface IResource
	{
		#region Open Methods
		/// <summary>
		/// Opens this resource for reading.
		/// </summary>
		/// <returns>Returns a <see cref="IOutputPipe"/>.</returns>
		IInputPipe OpenForReading();
		/// <summary>
		/// Opens this resource for writing.
		/// </summary>
		/// <returns>Returns a <see cref="IInputPipe"/>.</returns>
		IOutputPipe OpenForWriting();
		#endregion
		#region HashCode Methods
		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current <see cref="IResource"/>.</returns>
		string GetResourceIdentifier();
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path of this resource.
		/// </summary>
		IResourcePath Path { get; }
		/// <summary>
		/// Gets the size of this resource in bytes.
		/// </summary>
		long Length { get; }
		/// <summary>
		/// Gets the version of this resource.
		/// </summary>
		string Version { get; }
		#endregion
	}
}