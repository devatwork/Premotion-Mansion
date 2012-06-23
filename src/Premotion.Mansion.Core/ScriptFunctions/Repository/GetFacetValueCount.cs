using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Repository
{
	/// <summary>
	/// Gets the count of a given facet value.
	/// </summary>
	[ScriptFunction("GetFacetValueCount")]
	public class GetFacetValueCount : FunctionExpression
	{
		/// <summary>
		/// Gets the count of a given facet value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="result">The <see cref="FacetResult"/> in which to find the value count.</param>
		/// <param name="value">The value for which to get the count.</param>
		/// <returns>Returns the count if found, otherwise 0.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public int Evaluate(IMansionContext context, FacetResult result, string value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (result == null)
				throw new ArgumentNullException("result");
			if (value == null)
				throw new ArgumentNullException("value");

			// get the value
			var facetValue = result.Values.FirstOrDefault(candidate => value.Equals(candidate.Value.ToString(), StringComparison.OrdinalIgnoreCase));

			// return the count
			return facetValue != null ? facetValue.Count : 0;
		}
		/// <summary>
		/// Gets the count of a given facet value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="nodeset">The <see cref="Nodeset"/> from which to get the facet result.</param>
		/// <param name="propertyName">The name of the property for which to lookup the value.</param>
		/// <param name="value">The value for which to get the count.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="InvalidOperationException"></exception>
		public int Evaluate(IMansionContext context, Nodeset nodeset, string propertyName, string value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (nodeset == null)
				throw new ArgumentNullException("nodeset");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (value == null)
				throw new ArgumentNullException("value");

			// find the facet
			var facet = nodeset.Facets.First(candidate => propertyName.Equals(candidate.PropertyName, StringComparison.OrdinalIgnoreCase));
			if (facet == null)
				throw new InvalidOperationException(string.Format("Could not find facet for property '{0}' in the given nodeset", propertyName));

			//  return the value
			return Evaluate(context, facet, value);
		}
	}
}