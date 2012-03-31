using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Gets a special character.
	/// </summary>
	[ScriptFunction("Character")]
	public class Character : FunctionExpression
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
			if (input == null)
				throw new ArgumentNullException("input");

			// check for integers
			if (input.IsNumber())
			{
				var number = Convert.ToInt32(input);
				return Convert.ToChar(number).ToString(context.SystemCulture);
			}

			// check for special cases
			switch (input.ToLower())
			{
				case "newline":
					return "\r\n";
				case "singlequote":
					return "\'";
				case "doublequote":
					return "\"";
				case "backslash":
					return "\\";
				case "tab":
					return "\t";
				case "space":
					return " ";
			}

			// no match found
			throw new InvalidOperationException(string.Format("'{0}' is an unknown character code.", input));
		}
	}
}