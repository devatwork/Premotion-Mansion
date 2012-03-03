using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// URL encodes the input string.
	/// </summary>
	[ScriptFunction("UrlEncode")]
	public class UrlEncode : FunctionExpression
	{
		/// <summary>
		/// URL encodes the input string.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="input">The string which to encode.</param>
		public string Evaluate(MansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// return the url
			return HttpUtilities.UrlEncode(input);
		}
	}
}