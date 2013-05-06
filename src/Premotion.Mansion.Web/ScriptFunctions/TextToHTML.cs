using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Transforms th input text to html.
	/// </summary>
	[ScriptFunction("TextToHTML")]
	public class TextToHTML : FunctionExpression
	{
		/// <summary>
		/// Transforms th input text to html.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The string which to transform.</param>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// first strip any existing html
			var stripped = input.StripHtml();

			// transform to html
			return stripped.TextToHTML();
		}
	}
}