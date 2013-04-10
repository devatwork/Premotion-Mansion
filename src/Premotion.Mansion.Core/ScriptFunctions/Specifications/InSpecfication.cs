using System;
using System.Linq;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Specifications
{
	/// <summary>
	/// Creates a <see cref="IsPropertyInSpecification"/>.
	/// </summary>
	[ScriptFunction("InSpecification")]
	public class InSpecification : FunctionExpression
	{
		/// <summary>
		/// Creats a <see cref="IsPropertyInSpecification"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property on which to create the specification.</param>
		/// <param name="values">The values on which to check.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		public Specification Evaluate(IMansionContext context, string propertyName, string values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// split the values and clean them up
			var splittedValues = (values ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

			// create the specification
			return new IsPropertyInSpecification(propertyName, splittedValues);
		}
	}
}