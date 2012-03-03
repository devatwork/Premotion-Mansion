using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Returns the 
	/// </summary>
	[ScriptFunction("GetTimespanPart")]
	public class GetTimespanPart : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <param name="timeSpan"></param>
		/// <param name="part"></param>
		public double Evaluate(MansionContext context, TimeSpan timeSpan, string part)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (timeSpan == TimeSpan.MinValue)
				return double.NaN;
			if (string.IsNullOrEmpty(part))
				throw new ArgumentNullException("part");

			if ("day".Equals(part, StringComparison.OrdinalIgnoreCase))
				return System.Math.Round(timeSpan.TotalDays + 1, MidpointRounding.AwayFromZero);
			if ("hour".Equals(part, StringComparison.OrdinalIgnoreCase))
				return System.Math.Round(timeSpan.TotalHours + 1, MidpointRounding.AwayFromZero);
			if ("minute".Equals(part, StringComparison.OrdinalIgnoreCase))
				return System.Math.Round(timeSpan.TotalMinutes + 1, MidpointRounding.AwayFromZero);

			throw new InvalidOperationException(string.Format("{0} is not a valid timespan part", part));
		}
	}
}