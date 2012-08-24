using System;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Specifications
{
	/// <summary>
	/// Creates a <see cref="Negation"/>.
	/// </summary>
	[ScriptFunction("NotSpecification")]
	public class NotSpecification : FunctionExpression
	{
		/// <summary>
		/// Creats a <see cref="Negation"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to negate.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		public Specification Evaluate(IMansionContext context, Specification specification)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (specification == null)
				throw new ArgumentNullException("specification");

			// create the specification
			return SpecificationFactory.Not(specification);
		}
	}
}