using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Changes the value of an attribute in the query string.
	/// </summary>
	[ScriptFunction("ChangeQueryString")]
	public class ChangeQueryString : FunctionExpression
	{
		/// <summary>
		/// Changes the value of an attribute in the query string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="url">The <see cref="Uri"/> which to change.</param>
		/// <param name="parameterName">The name of the parmeters which to set.</param>
		/// <param name="value">The value of the parameters.</param>
		/// <returns>The <see cref="Uri"/> with the modified query string.</returns>
		public Uri Evaluate(IMansionContext context, Uri url, string parameterName, string value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");
			if (string.IsNullOrEmpty(parameterName))
				throw new ArgumentNullException("parameterName");

			var modifiableUri = new UriBuilder(url);

			// parse the query string
			var parsedQueryString = modifiableUri.Query.ParseQueryString();

			// set the property
			parsedQueryString.Set(parameterName, value);

			// set the new query string
			modifiableUri.Query = parsedQueryString.ToHttpSafeString();

			// return the url
			return modifiableUri.Uri;
		}
	}
}