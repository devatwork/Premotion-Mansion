using System;
using Premotion.Mansion.Core.Data.Facets;
using Premotion.Mansion.Core.Scripting;
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
		public FacetDefinition Evaluate(IMansionContext context, string propertyName, string friendlyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(friendlyName))
				throw new ArgumentNullException("friendlyName");

			// create the facet
			return new FacetDefinition(propertyName, friendlyName);
		}
		/// <summary>
		/// Creates a <see cref="Facet"/> from the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property on which to facet.</param>
		/// <param name="friendlyName">The friendly name of the property.</param>
		/// <param name="transformProcedureName">The name of the transform procedure which to invoke for each of the facets value.</param>
		/// <returns>Returns the created <see cref="Facet"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null or empty.</exception>
		public FacetDefinition Evaluate(IMansionContext context, string propertyName, string friendlyName, string transformProcedureName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (string.IsNullOrEmpty(friendlyName))
				throw new ArgumentNullException("friendlyName");
			if (string.IsNullOrEmpty(transformProcedureName))
				throw new ArgumentNullException("transformProcedureName");

			// get the transform procedure
			IScript procedure;
			if (!context.ProcedureStack.TryPeek(transformProcedureName, out procedure))
				throw new InvalidOperationException(string.Format("No procedure found with name '{0}'", transformProcedureName));

			// create the facet
			return new TransformingFacetDefinition(propertyName, friendlyName, procedure);
		}
	}
}