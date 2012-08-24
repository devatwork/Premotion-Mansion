using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Queries
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
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override sealed string ToString()
		{
			var builder = new StringBuilder();

			// loop over all the components
			var first = true;
			foreach (var component in Components)
			{
				// append separator if not first
				if (first)
					first = false;
				else
					builder.Append('_');

				// append component to builder
				component.AsString(builder);
			}

			return builder.ToString();
		}
		#endregion
		#region Private Fields
		private readonly Queue<QueryComponent> componentQueue = new Queue<QueryComponent>();
		private readonly List<ITypeDefinition> typeHints = new List<ITypeDefinition>();
		#endregion
	}
}