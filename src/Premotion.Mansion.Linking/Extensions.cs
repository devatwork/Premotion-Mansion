using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides extension methods used by the link module.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Filters <paramref name="this"/> to only include links to <paramref name="target"/>.
		/// </summary>
		/// <param name="this">The <see cref="Link"/>s which to filter.</param>
		/// <param name="target">The target <see cref="Linkbase"/>.</param>
		/// <returns>Returns the filtered <see cref="Link"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static IEnumerable<Link> To(this IEnumerable<Link> @this, Linkbase target)
		{
			//  validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (target == null)
				throw new ArgumentNullException("target");
			return @this.Where(candidate => candidate.IsTo(target));
		}
		/// <summary>
		/// Filters <paramref name="this"/> to only include links of type <paramref name="definition"/>.
		/// </summary>
		/// <param name="this">The <see cref="Link"/>s which to filter.</param>
		/// <param name="definition">The <see cref="LinkDefinition"/>.</param>
		/// <returns>Returns the filtered <see cref="Link"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static IEnumerable<Link> OfType(this IEnumerable<Link> @this, LinkDefinition definition)
		{
			//  validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (definition == null)
				throw new ArgumentNullException("definition");
			return @this.Where(candidate => candidate.IsInstanceOf(definition));
		}
	}
}