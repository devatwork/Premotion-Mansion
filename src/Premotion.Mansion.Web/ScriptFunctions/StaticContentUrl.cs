using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a path for static content.
	/// </summary>
	[ScriptFunction("StaticContentUrl")]
	public class StaticContentUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a path for static content.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="relativePath">The relative path to the resource.</param>
		/// <returns>The <see cref="Uri"/> ot the static content.</returns>
		public Uri Evaluate(IMansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				return null;

			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// create the relative path
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, StaticContentRequestHandler.Prefix, relativePath);

			// create the uri
			return new Uri(webContext.ApplicationBaseUri, prefixedRelativePath);
		}
	}
}