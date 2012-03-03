using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Turns a dataspace into a query string.
	/// </summary>
	[ScriptFunction("DataspaceToQueryString")]
	public class DataspaceToQueryString : FunctionExpression
	{
		/// <summary>
		/// Changes the value of an attribute in the query string.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="url">The <see cref="Uri"/> which to change.</param>
		/// <param name="dataspace">The <see cref="IPropertyBag"/> which to add to the query string.</param>
		/// <returns>The <see cref="Uri"/> with the modified query string.</returns>
		public Uri Evaluate(MansionContext context, Uri url, IPropertyBag dataspace)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");
			if (dataspace == null)
				throw new ArgumentNullException("dataspace");

			var modifiableUri = new UriBuilder(url);

			// parse the query string
			var parsedQueryString = modifiableUri.Query.ParseQueryString();

			// merge the dataspace
			parsedQueryString.Merge(dataspace);

			// set the new query string
			modifiableUri.Query = parsedQueryString.ToHttpSafeString();

			// return the url
			return modifiableUri.Uri;
		}
	}
}