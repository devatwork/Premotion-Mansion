using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Adds a number of days to the date.
	/// </summary>
	[ScriptFunction("TimeAdd")]
	public class TimeAdd : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <param name="input"></param>
		/// <param name="hours"></param>
		/// <param name="minutes"></param>
		/// <param name="seconds"></param>
		public DateTime Evaluate(IMansionContext context, DateTime input, int hours, int minutes, int seconds)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == DateTime.MinValue)
				return DateTime.MinValue;

			return input.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
		}
	}
}