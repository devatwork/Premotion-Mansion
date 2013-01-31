using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Clips the input string to the specified number of chararcters.
	/// </summary>
	[ScriptFunction("Trim")]
	public class Trim : FunctionExpression
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

			return input == null ? string.Empty : input.Trim();
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <param name="trimChars"> </param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input, params char[] trimChars)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return input == null ? string.Empty : input.Trim(trimChars);
		}
	}
}