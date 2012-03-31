using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Strips all HTML from the input string.
	/// </summary>
	[ScriptFunction("StripHTML")]
	public class StripHTML : FunctionExpression
	{
		/// <summary>
		/// Strips all HTML from the input string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The string which to strip.</param>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// return the result
			return input.StripHtml();
		}
	}
}