﻿using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Implements the if function.
	/// </summary>
	[ScriptFunction("If")]
	public class If : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="condition"></param>
		/// <param name="trueValue"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, bool condition, string trueValue)
		{
			return Evaluate(context, condition, trueValue, string.Empty);
		}
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="condition"></param>
		/// <param name="trueValue"></param>
		/// <param name="falseValue"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, bool condition, string trueValue, string falseValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return condition ? trueValue : falseValue;
		}
	}
}