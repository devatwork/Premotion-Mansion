using System.IO;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a stream from which can be read.
	/// </summary>
	public interface IInputPipe : IPipe
	{
		#region Properties
		/// <summary>
		/// Gets the reader for this pipe.
		/// </summary>
		TextReader Reader { get; }
		/// <summary>
		/// Gets the underlying stream of this pipe. Use with caution.
		/// </summary>
		Stream RawStream { get; }
		#endregion
	}
}