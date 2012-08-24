using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Patterns.Prioritized
{
	/// <summary>
	/// Provides extension for prioritized objects.
	/// </summary>
	public static class PrioritizedExtensions
	{
		#region Extensions of IEnumerable{IPrioritized}
		/// <summary>
		/// Orders the objects in the given <paramref name="set"/> to their relative priority. The higher the priority the earlies it is returned.
		/// </summary>
		/// <param name="set">The <see cref="IEnumerable{T}"/> which to order.</param>
		/// <typeparam name="TPrioritized">The type of object. Must implement <see cref="IPrioritized"/>.</typeparam>
		/// <returns>Returns the ordered set.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="set"/> is null.</exception>
		public static IEnumerable<TPrioritized> OrderByPriority<TPrioritized>(this IEnumerable<TPrioritized> set) where TPrioritized : IPrioritized
		{
			// validate arguments
			if (set == null)
				throw new ArgumentNullException("set");

			// reoder
			return set.OrderByDescending(obj => obj.Priority);
		}
		#endregion
	}
}