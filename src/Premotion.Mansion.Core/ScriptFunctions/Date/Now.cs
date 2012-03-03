using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Date
{
	/// <summary>
	/// Returns the current date and time.
	/// </summary>
	[ScriptFunction("Now")]
	public class Now : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public DateTime Evaluate(MansionContext context)
		{
			return DateTime.Now;
		}
	}
}