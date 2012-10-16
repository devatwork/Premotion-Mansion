using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Concatenates the input into one long string.
	/// </summary>
	[ScriptFunction("Concat")]
	public class Concat : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parts"> </param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, params string[] parts)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parts == null)
				throw new ArgumentNullException("parts");

			return string.Join(string.Empty, parts);
		}
	}
}