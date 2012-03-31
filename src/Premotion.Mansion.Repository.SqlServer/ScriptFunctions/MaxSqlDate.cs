using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Repository.SqlServer.ScriptFunctions
{
	/// <summary>
	/// Returns the current date.
	/// </summary>
	[ScriptFunction("MaxSqlDate")]
	public class MaxSqlDate : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public DateTime Evaluate(IMansionContext context)
		{
			return new DateTime(2037, 12, 31);
		}
	}
}