using System;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Specifications
{
	/// <summary>
	/// Creates a <see cref="Disjunction"/>.
	/// </summary>
	[ScriptFunction("OrSpecification")]
	public class OrSpecification : FunctionExpression
	{
		/// <summary>
		/// Creats a <see cref="Negation"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specifications">The <see cref="Specification"/> which to negate.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		public Specification Evaluate(IMansionContext context, params Specification[] specifications)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (specifications == null)
				throw new ArgumentNullException("specifications");

			// create the specification
			return SpecificationFactory.Or(specifications);
		}
	}
}