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
		/// <param name="source">The source <see cref="Uri"/> from which to copy the query string.</param>
		/// <param name="target">The target <see cref="Uri"/> to which to copy the query string.</param>
		/// <returns>The <see cref="Uri"/> with the modified query string.</returns>
		public Uri Evaluate(IMansionContext context, Uri source, Uri target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");

			return new UriBuilder(target)
			       {
			       	Query = source.Query.TrimStart(new[] {'?'}),
			       	Fragment = source.Fragment
			       }.Uri;
		}
	}
}