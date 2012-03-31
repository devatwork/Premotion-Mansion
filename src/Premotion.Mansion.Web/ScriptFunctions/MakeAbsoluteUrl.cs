using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates an absulute url from the <see cref="Uri"/>.
	/// </summary>
	[ScriptFunction("MakeAbsoluteUrl")]
	public class MakeAbsoluteUrl : FunctionExpression
	{
		/// <summary>
		/// Generates an absulute url from the <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="url">The <see cref="Uri"/> which to make absolute.</param>
		/// <returns>The <see cref="Uri"/> ot the static resource.</returns>
		public string Evaluate(IMansionContext context, Uri url)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");

			// if the url is an absolute url, return it
			if (url.IsAbsoluteUri)
				return url.ToString();

			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// create the uri
			return new Uri(webContext.ApplicationBaseUri, url).ToString();
		}
	}
}