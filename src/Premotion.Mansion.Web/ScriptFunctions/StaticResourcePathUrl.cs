using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a path for static resource paths.
	/// </summary>
	[ScriptFunction("StaticResourcePathUrl")]
	public class StaticResourcePathUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a path for static resources.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="relativePath">The relative path to the resource.</param>
		/// <returns>The <see cref="Uri"/> ot the static resource.</returns>
		public Uri Evaluate(MansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			// get the web context
			var webContext = context.Cast<MansionWebContext>();

			// create the relative path
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, PathRewriterModule.StaticResourcesPrefix, relativePath);

			// create the uri
			return new Uri(webContext.HttpContext.Request.ApplicationBaseUri, prefixedRelativePath + "/");
		}
	}
}