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
		/// <param name="url">The <see cref="Url"/> which to change.</param>
		/// <param name="parameterName">The name of the parmeters which to set.</param>
		/// <param name="value">The value of the parameters.</param>
		/// <returns>The <see cref="Url"/> with the modified query string.</returns>
		public Url Evaluate(IMansionContext context, Url url, string parameterName, string value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");
			if (string.IsNullOrEmpty(parameterName))
				throw new ArgumentNullException("parameterName");

			var modifiableUri = url.Clone();

			// set the property
			modifiableUri.QueryString[parameterName] = value;

			// return the url
			return modifiableUri;
		}
	}
}