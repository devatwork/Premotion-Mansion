using System;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Specifications
{
	/// <summary>
	/// Appends specifications to a composite.
	/// </summary>
	[ScriptFunction("AppendSpecifications")]
	public class AppendSpecifications : FunctionExpression
	{
		/// <summary>
		/// Appends <paramref name="specifications"/> to <paramref name="composite"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="composite">The <see cref="CompositeSpecification"/> to which to add the <paramref name="specifications"/>.</param>
		/// <param name="specifications">The <see cref="Specification"/> which to negate.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		public Specification Evaluate(IMansionContext context, CompositeSpecification composite, params Specification[] specifications)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (composite == null)
				throw new ArgumentNullException("composite");
			if (specifications == null)
				throw new ArgumentNullException("specifications");

			// create the specification
			return composite.Add(specifications);
		}
	}
}