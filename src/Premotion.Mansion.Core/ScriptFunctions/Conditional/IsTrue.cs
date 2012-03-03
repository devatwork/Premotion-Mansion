﻿using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether the value is true, otherwise it will return false.
	/// </summary>
	[ScriptFunction("IsTrue")]
	public class IsTrue : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public bool Evaluate(MansionContext context, object input)
		{
			// get the conversion service
			var conversionService = context.Nucleus.Get<IConversionService>(context);

			// convert the value
			return conversionService.Convert(context, input, false);
		}
	}
}