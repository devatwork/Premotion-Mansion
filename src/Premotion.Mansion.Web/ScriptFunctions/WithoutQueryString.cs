using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Gets the <see cref="Uri"/> without the query string.
	/// </summary>
	[ScriptFunction("WithoutQueryString")]
	public class WithoutQueryString : FunctionExpression
	{
		/// <summary>
		/// Gets the <see cref="Uri"/> without the query string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="Uri"/> from which to copy the query string.</param>
		/// <returns>The <see cref="Uri"/> with the modified query string.</returns>
		public Uri Evaluate(IMansionContext context, Uri source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");

			return new Uri(source.GetLeftPart(UriPartial.Path));
		}
	}
}