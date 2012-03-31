using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Makes the first charachter of the input string upper case.
	/// </summary>
	[ScriptFunction("ToPascalCase")]
	public class ToPascalCase : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			if (string.IsNullOrEmpty(input))
				return null;

			return Char.ToUpper(input[0]) + input.Substring(1);
		}
	}
}