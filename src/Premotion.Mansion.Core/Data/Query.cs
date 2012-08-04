using System;
using System.Collections.Generic;
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
		/// Adds a new <paramref name="component"/> to this query.
		/// </summary>
		/// <param name="component">The <see cref="QueryComponent"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="component"/> is null.</exception>
		public void Add(QueryComponent component)
		{
			// validate arguments
			if (component == null)
				throw new ArgumentNullException("component");

			// add to the list
			componentQueue.Enqueue(component);
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
		/// Gets the <see cref="QueryComponent"/>s of this query.
		/// </summary>
		public IEnumerable<QueryComponent> Components
		{
			get { return componentQueue; }
		}
		/// <summary>
		/// Gets the hinted <see cref="ITypeDefinition"/>s.
		/// </summary>
		public IEnumerable<ITypeDefinition> TypeHints
		{
			get { return typeHints; }
		}
		#endregion
		#region Private Fields
		private readonly Queue<QueryComponent> componentQueue = new Queue<QueryComponent>();
		private readonly List<ITypeDefinition> typeHints = new List<ITypeDefinition>();
		#endregion
	}
}