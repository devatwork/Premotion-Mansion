using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Adds apostrofs to CSV.
	/// </summary>
	[ScriptFunction("AddAposToCsv")]
	public class AddAposToCsv : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			// split the values
			var values = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => '\'' + x.Trim() + '\'').ToArray();

			// join the values
			return string.Join(",", values);
		}
	}
}