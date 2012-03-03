using System;
using System.Text;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a stream pipe.
	/// </summary>
	public interface IPipe : IDisposable
	{
		#region Properties
		/// <summary>
		/// Gets the encoding of this pipe.
		/// </summary>
		Encoding Encoding { get; }
		#endregion
	}
}