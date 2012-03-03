using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates an relative url from the <see cref="Uri"/>.
	/// </summary>
	[ScriptFunction("MakeRelativeUrl")]
	public class MakeRelativeUrl : FunctionExpression
	{
		/// <summary>
		/// Generates an absulute url from the <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="url">The <see cref="Uri"/> which to make absolute.</param>
		/// <returns>The <see cref="Uri"/> ot the static resource.</returns>
		public Uri Evaluate(MansionContext context, Uri url)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");

			// if the url is an relative url, return it
			if (!url.IsAbsoluteUri)
				return url;

			// get the web context
			var webContext = context.Cast<MansionWebContext>();

			// create the uri
			return webContext.HttpContext.Request.ApplicationBaseUri.MakeRelativeUri(url);
		}
	}
}