using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Conditional
{
	/// <summary>
	/// Checks whether a value is in a list.
	/// </summary>
	[ScriptFunction("InList")]
	public class InList : FunctionExpression
	{
		/// <summary>
		/// Checks whether <paramref name="needle"/> is in <paramref name="haystack"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="needle">The needle for which to look.</param>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public bool Evaluate(MansionContext context, string needle, string haystack)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (needle == null)
				needle = string.Empty;
			if (haystack == null)
				haystack = string.Empty;

			return haystack.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Any(candidate => needle.Equals(candidate.Trim(), StringComparison.OrdinalIgnoreCase));
		}
	}
}