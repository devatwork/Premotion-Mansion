using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Gets the number of ticks that represent the value of the current TimeSpan structure.
	/// </summary>
	[ScriptFunction("Ticks")]
	public class Ticks : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <param name="input"></param>
		public long Evaluate(IMansionContext context, DateTime input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == DateTime.MinValue)
				return DateTime.MinValue.Ticks;

			return input.Ticks;
		}
	}
}