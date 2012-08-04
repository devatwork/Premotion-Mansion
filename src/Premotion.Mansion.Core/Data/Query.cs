using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Specifications;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents an abstract query which can be used to retrieve data from a <see cref="IRepository"/>.
	/// </summary>
	public class Query
	{
		#region Add Methods
		/// <summary>
		/// Adds a new <paramref name="specification"/> to this query. Specifications are by default combined using the AND operator.
		/// </summary>
		/// <param name="specification">The <see cref="Specification"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		public void Add(Specification specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// add to the list
			specificationQueue.Enqueue(specification);
		}
		/// <summary>
		/// Adds a new <paramref name="sort"/> to this query.
		/// </summary>
		/// <param name="sort">The <see cref="Sort"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sort"/> is null.</exception>
		public void Add(Sort sort)
		{
			// validate arguments
			if (sort == null)
				throw new ArgumentNullException("sort");

			// add the sort
			sortQueue.Enqueue(sort);
		}
		/// <summary>
		/// Adds a new <paramref name="sorts"/> to this query.
		/// </summary>
		/// <param name="sorts">The <see cref="Sort"/>s which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sorts"/> is null.</exception>
		public void Add(IEnumerable<Sort> sorts)
		{
			// validate arguments
			if (sorts == null)
				throw new ArgumentNullException("sorts");

			// add the sort
			foreach (var sort in sorts)
				sortQueue.Enqueue(sort);
		}
		/// <summary>
		/// Adds the given <paramref name="types"/> as type hints to this query.
		/// </summary>
		/// <param name="types">The <see cref="ITypeDefinition"/>s which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null.</exception>
		public void Add(IEnumerable<ITypeDefinition> types)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");

			// add all new types
			typeHints.AddRange(types);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the hinted <see cref="ITypeDefinition"/>s.
		/// </summary>
		public IEnumerable<ITypeDefinition> TypeHints
		{
			get { return typeHints; }
		}
		#endregion
		#region Private Fields
		private readonly Queue<Sort> sortQueue = new Queue<Sort>();
		private readonly Queue<Specification> specificationQueue = new Queue<Specification>();
		private readonly List<ITypeDefinition> typeHints = new List<ITypeDefinition>();
		#endregion
	}
}