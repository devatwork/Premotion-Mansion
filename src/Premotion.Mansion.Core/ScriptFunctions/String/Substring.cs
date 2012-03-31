using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Gets a portion of a string.
	/// </summary>
	[ScriptFunction("Substring")]
	public class Substring : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input, int startIndex)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			return input.Substring(startIndex);
		}
	}
}