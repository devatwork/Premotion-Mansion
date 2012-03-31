using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Returns the 
	/// </summary>
	[ScriptFunction("DateDiff")]
	public class DateDiff : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		/// <param name="one"></param>
		/// <param name="other"></param>
		public TimeSpan Evaluate(IMansionContext context, DateTime one, DateTime other)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (one == DateTime.MinValue)
				return TimeSpan.MinValue;
			if (other == DateTime.MinValue)
				return TimeSpan.MinValue;

			return one - other;
		}
	}
}