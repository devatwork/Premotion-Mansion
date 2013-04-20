using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.List
{
	/// <summary>
	/// Appends the given needle to the haystack.
	/// </summary>
	[ScriptFunction("AppendNeedle")]
	public class AppendNeedle : FunctionExpression
	{
		/// <summary>
		/// Appends the given needle to the haystack.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="needle">The needle which to remove.</param>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <returns>Returns the modified list.</returns>
		public string Evaluate(IMansionContext context, string needle, string haystack)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (needle == null)
				needle = string.Empty;
			if (haystack == null)
				haystack = string.Empty;
			return haystack.AppendNeedle(needle);
		}
	}
}