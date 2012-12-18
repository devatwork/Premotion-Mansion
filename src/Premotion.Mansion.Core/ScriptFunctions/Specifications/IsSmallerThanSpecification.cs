using System;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Specifications
{
	/// <summary>
	/// Creates a <see cref="IsPropertySmallerThanSpecification"/>.
	/// </summary>
	[ScriptFunction("IsSmallerThanSpecification")]
	public class IsSmallerThanSpecification : FunctionExpression
	{
		/// <summary>
		/// Creats a <see cref="IsPropertySmallerThanSpecification"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="propertyName">The name of the property on which to create the specification.</param>
		/// <param name="value">The value on which to check.</param>
		/// <returns>Returns the created <see cref="Specification"/>.</returns>
		public Specification Evaluate(IMansionContext context, string propertyName, object value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// create the specification
			return new IsPropertySmallerThanSpecification(propertyName, value);
		}
	}
}