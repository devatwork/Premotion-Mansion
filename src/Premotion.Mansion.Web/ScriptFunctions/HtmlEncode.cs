using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// HTML encodes the input string.
	/// </summary>
	[ScriptFunction("HtmlEncode")]
	public class HtmlEncode : FunctionExpression
	{
		/// <summary>
		/// HTML encodes the input string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The string which to encode.</param>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			return input.HtmlEncode();
		}
	}
}