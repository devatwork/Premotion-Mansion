using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.List
{
	/// <summary>
	/// Removes the given needle from the haystack.
	/// </summary>
	[ScriptFunction("RemoveNeedle")]
	public class RemoveNeedle : FunctionExpression
	{
		/// <summary>
		/// Removes the given needle from the haystack.
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

			// split the lists
			var list = haystack.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

			// reassemble the list without the needle
			return string.Join(",", list.Except(new[] {needle}, StringComparer.OrdinalIgnoreCase));
		}
	}
}