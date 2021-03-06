﻿using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Returns an apostrophe escaped string
	/// </summary>
	[ScriptFunction("EscapeApos")]
	public class EscapeApos : FunctionExpression
	{
		/// <summary>
		/// Returns an apostrophe escaped string
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input)
		{
			return input.Replace("'", "\\'");
		}
	}
}