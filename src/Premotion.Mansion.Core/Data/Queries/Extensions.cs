using System;
using System.Collections.Generic;
using System.Linq;
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
		/// <summary>
		/// Tries to get the <typeparamref name="TQueryComponent"/> from the <paramref name="query"/>.
		/// </summary>
		/// <typeparam name="TQueryComponent">The type of <see cref="QueryComponent"/> which to get.</typeparam>
		/// <param name="query">The <see cref="Query"/> from which to get the component.</param>
		/// <param name="component">The <typeparamref name="TQueryComponent"/> which to get.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
		public static bool TryGetComponent<TQueryComponent>(this Query query, out TQueryComponent component) where TQueryComponent : QueryComponent
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// loop over all the types
			component = query.Components.OfType<TQueryComponent>().FirstOrDefault();
			return component != null;
		}
		#endregion
	}
}