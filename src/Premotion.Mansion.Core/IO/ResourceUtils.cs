using System.IO;
using System.Linq;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Provides utility functions for resources.
	/// </summary>
	public static class ResourceUtils
	{
		/// <summary>
		/// Combines several parts into a path.
		/// </summary>
		/// <param name="parts">The path parts.</param>
		/// <returns>Returns the combinded path.</returns>
		public static string Combine(params string[] parts)
		{
			// TODO: make paths safe
			return parts.Aggregate(string.Empty, (current, part) => Path.Combine(current, part.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar).Trim(Path.DirectorySeparatorChar)));
		}
	}
}