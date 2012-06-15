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
		/// Constructs a facet result.
		/// </summary>
		/// <param name="facet">The <see cref="Facet"/> for which to construct the result.</param>
		/// <param name="values">The <see cref="FacetValue"/>s.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public FacetResult(Facet facet, IEnumerable<FacetValue> values) : base(facet)
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");

			// set the values
			Values = values.OrderByDescending(value => value.Count).ToList();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the resulting <see cref="FacetValue"/>s.
		/// </summary>
		public IEnumerable<FacetValue> Values { get; set; }
		#endregion
	}
}