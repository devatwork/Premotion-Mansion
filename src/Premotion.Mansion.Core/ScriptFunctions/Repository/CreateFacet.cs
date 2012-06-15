using System;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Repository
{
	/// <summary>
	/// Creates a <see cref="Facet"/> from a given property name.
	/// </summary>
	[ScriptFunction("CreateFacet")]
	public class CreateFacet : FunctionExpression
	{
		/// <summary>
		/// Creates a <see cref="Facet"/> from the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property on which to facet.</param>
		/// <param name="friendlyName">The friendly name of the property.</param>
		/// <returns>Returns the created <see cref="Facet"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public Facet Evaluate(IMansionContext context, string propertyName, string friendlyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(friendlyName))
				throw new ArgumentNullException("friendlyName");

			// create the facet
			return new Facet(propertyName, friendlyName);
		}
	}
}