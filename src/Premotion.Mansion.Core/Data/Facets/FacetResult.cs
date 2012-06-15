using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data.Facets
{
	/// <summary>
	/// Represents the result of a <see cref="Facet"/> query.
	/// </summary>
	public class FacetResult : Facet
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="friendlyName"></param>
		/// <param name="values"></param>
		private FacetResult(string propertyName, string friendlyName, IEnumerable<FacetValue> values) : base(propertyName, friendlyName)
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");

			// set the values
			this.values = values.ToList();
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Construcs a <see cref="FacetResult"/> from the given <paramref name="definition"/> and <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="FacetDefinition"/>.</param>
		/// <param name="values">The <see cref="FacetValue"/>s.</param>
		/// <returns>Returns the created <see cref="FacetResult"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static FacetResult Create(IMansionContext context, FacetDefinition definition, IEnumerable<FacetValue> values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (definition == null)
				throw new ArgumentNullException("definition");
			if (values == null)
				throw new ArgumentNullException("values");

			// transform the result using the definition
			var transformedResults = definition.Transform(context, values);

			// create the facet result
			return new FacetResult(definition.PropertyName, definition.FriendlyName, transformedResults);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the resulting <see cref="FacetValue"/>s.
		/// </summary>
		public IEnumerable<FacetValue> Values
		{
			get { return values; }
		}
		/// <summary>
		/// Gets the number of values within this result.
		/// </summary>
		public int ValueCount
		{
			get { return values.Count; }
		}
		#endregion
		#region Private Fields
		private readonly ICollection<FacetValue> values;
		#endregion
	}
}