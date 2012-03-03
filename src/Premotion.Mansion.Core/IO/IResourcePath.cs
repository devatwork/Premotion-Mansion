using System.Collections.Generic;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents a resource path.
	/// </summary>
	public interface IResourcePath
	{
		#region Properties
		/// <summary>
		/// Gets a flag indicating whether this resource is overridable or not.
		/// </summary>
		bool Overridable { get; }
		/// <summary>
		/// Gets the relative path to this resource.
		/// </summary>
		IEnumerable<string> Paths { get; }
		#endregion
	}
}