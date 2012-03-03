using System.IO;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a stream to which to write.
	/// </summary>
	public interface IOutputPipe : IPipe
	{
		#region Properties
		/// <summary>
		/// Gets the writer for this pipe.
		/// </summary>
		TextWriter Writer { get; }
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		Stream RawStream { get; }
		#endregion
	}
}