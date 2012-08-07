using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Defines extension methods for several types.
	/// </summary>
	public static class Extensions
	{
		#region Extensions of Query
		/// <summary>
		/// Adds the <paramref name="specification"/> to the <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <param name="specification">The <see cref="Specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="specification"/> is null.</exception>
		public static void Add(this Query query, Specification specification)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");
			if (specification == null)
				throw new ArgumentNullException("specification");

			// wrap the specification in a component
			var component = new SpecificationQueryComponent(specification);

			// add the component to the query
			query.Add(component);
		}
		/// <summary>
		/// Adds the <paramref name="sorts"/> to the <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <param name="sorts">The <see cref="Specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="sorts"/> is null.</exception>
		public static void Add(this Query query, IEnumerable<Sort> sorts)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");
			if (sorts == null)
				throw new ArgumentNullException("sorts");

			// wrap the specification in a component
			var component = new SortQueryComponent(sorts);

			// add the component to the query
			query.Add(component);
		}
		#endregion
	}
}