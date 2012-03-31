using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Repository.SqlServer.ScriptFunctions
{
	/// <summary>
	/// SQL encodes the input.
	/// </summary>
	[ScriptFunction("SqlEncode")]
	public class SqlEncode : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (input == null)
				throw new ArgumentNullException("input");

			return input.Replace("'", "''");
		}
	}
}