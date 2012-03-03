using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Makes the first charachter of the input string lower case.
	/// </summary>
	[ScriptFunction("ToCamelCase")]
	public class ToCamelCase : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			if (string.IsNullOrEmpty(input))
				return null;

			return Char.ToLower(input[0]) + input.Substring(1);
		}
	}
}