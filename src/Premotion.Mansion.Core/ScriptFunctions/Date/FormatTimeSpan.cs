using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Implements the format date function.
	/// </summary>
	[ScriptFunction("FormatTimeSpan")]
	public class FormatTimeSpan : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="time"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, TimeSpan time, string format)
		{
			// validate arguments
			if (time == TimeSpan.MinValue)
				return string.Empty;

			// return the formated date
			return time.ToString(format, context.UserInterfaceCulture);
		}
	}
}