using System;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Check whether a string contains another string.
	/// </summary>
	[ScriptFunction("Contains")]
	public class Contains : FunctionExpression
	{
		/// <summary>
		/// Checks whether <paramref name="haystack"/> contains the <paramref name="needle"/> using a case insensitive comparison.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <param name="needle">The needle for which to look.</param>
		/// <returns>Returns true when <paramref name="haystack"/> contains the <paramref name="needle"/>, otherwise false.</returns>
		public bool Evaluate(MansionContext context, string haystack, string needle)
		{
			return Evaluate(context, haystack, needle, false);
		}
		/// <summary>
		/// Checks whether <paramref name="haystack"/> contains the <paramref name="needle"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <param name="needle">The needle for which to look.</param>
		/// <param name="caseSensitive">Flag indicating whether to perform a case sensitive or case insensitive comparison.</param>
		/// <returns>Returns true when <paramref name="haystack"/> contains the <paramref name="needle"/>, otherwise false.</returns>
		public bool Evaluate(MansionContext context, string haystack, string needle, bool caseSensitive)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(haystack))
				return false;
			if (string.IsNullOrEmpty(needle))
				return false;

			// check if the needle is in the hay stack
			return haystack.IndexOf(needle, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) > -1;
		}
	}
}