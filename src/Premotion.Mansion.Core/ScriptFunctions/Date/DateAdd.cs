using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Adds a number of days to the date.
	/// </summary>
	[ScriptFunction("DateAdd")]
	public class DateAdd : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <param name="input"></param>
		/// <param name="years"></param>
		/// <param name="months"></param>
		/// <param name="days"></param>
		public DateTime Evaluate(MansionContext context, DateTime input, int years, int months, int days)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == DateTime.MinValue)
				return DateTime.MinValue;

			return input.AddYears(years).AddMonths(months).AddDays(days);
		}
	}
}