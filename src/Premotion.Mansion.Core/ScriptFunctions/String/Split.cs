using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Splits a given string.
	/// </summary>
	[ScriptFunction("Split")]
	public class Split : FunctionExpression
	{
		/// <summary>
		/// Splits a given string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to split.</param>
		/// <returns>Returns the splitted parts.</returns>
		public string[] Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return Evaluate(context, input, ",");
		}
		/// <summary>
		/// Splits a given string.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input">The input which to split.</param>
		/// <param name="separator">The seperator which to use.</param>
		/// <returns>Returns the splitted parts.</returns>
		public string[] Evaluate(IMansionContext context, string input, string separator)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return (input ?? string.Empty).Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}