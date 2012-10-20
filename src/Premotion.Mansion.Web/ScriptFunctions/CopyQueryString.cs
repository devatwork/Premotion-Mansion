using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Copies the query string from one <see cref="Uri"/> to another.
	/// </summary>
	[ScriptFunction("CopyQueryString")]
	public class CopyQueryString : FunctionExpression
	{
		/// <summary>
		/// Copies the query string from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Url"/> from which to copy the query string.</param>
		/// <param name="target">The target <see cref="Url"/> to which to copy the query string.</param>
		/// <returns>The <see cref="Url"/> with the modified query string.</returns>
		public Url Evaluate(IMansionContext context, Url source, Url target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");

			var clone = target.Clone();

			foreach (var entry in source.QueryString)
				clone.QueryString[entry.Key] = entry.Value;

			return clone;
		}
	}
}