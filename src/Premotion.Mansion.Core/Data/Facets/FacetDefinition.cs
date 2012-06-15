using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Data.Facets
{
	/// <summary>
	/// Represents a definition of a <see cref="Facet"/>.
	/// </summary>
	public class FacetDefinition : Facet
	{
		#region Constructors
		/// <summary>
		/// Creates a facet with the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to facet.</param>
		/// <param name="friendlyName">The friendly name.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public FacetDefinition(string propertyName, string friendlyName) : base(propertyName, friendlyName)
		{
		}
		#endregion
		#region Transform Methods
		/// <summary>
		/// Transforms the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="values">The <see cref="FacetValue"/>s which to transform.</param>
		/// <returns>Returns the transformed <see cref="FacetValue"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public IEnumerable<FacetValue> Transform(IMansionContext context, IEnumerable<FacetValue> values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (values == null)
				throw new ArgumentNullException("values");

			// invoke template method
			return DoTransform(context, values);
		}
		/// <summary>
		/// Transforms the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="values">The <see cref="FacetValue"/>s which to transform.</param>
		/// <returns>Returns the transformed <see cref="FacetValue"/>s.</returns>
		protected virtual IEnumerable<FacetValue> DoTransform(IMansionContext context, IEnumerable<FacetValue> values)
		{
			// just sort the results
			return values.OrderByDescending(value => value.Count);
		}
		#endregion
	}
}