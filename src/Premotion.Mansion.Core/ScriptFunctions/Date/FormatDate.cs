using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Implements the format date function.
	/// </summary>
	[ScriptFunction("FormatDate")]
	public class FormatDate : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="date"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, DateTime date, string format)
		{
			// validate arguments
			if (date == DateTime.MinValue)
				return string.Empty;

			// return the formated date
			return date.ToString(format, context.UserInterfaceCulture);
		}
	}
}