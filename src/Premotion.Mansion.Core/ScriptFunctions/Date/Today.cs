using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Returns the current date.
	/// </summary>
	[ScriptFunction("Today")]
	public class Today : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public DateTime Evaluate(MansionContext context)
		{
			return DateTime.Today;
		}
	}
}