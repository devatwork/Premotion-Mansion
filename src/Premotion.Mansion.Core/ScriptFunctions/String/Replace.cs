using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.String
{
	/// <summary>
	/// Replaces a substring.
	/// </summary>
	[ScriptFunction("Replace")]
	public class Replace : FunctionExpression
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="input"> </param>
		/// <param name="oldValue"> </param>
		/// <param name="newValue"> </param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string input, string oldValue, string newValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return (input ?? string.Empty).Replace(oldValue, newValue);
		}
	}
}