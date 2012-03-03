using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a path for static resources.
	/// </summary>
	[ScriptFunction("StaticResourceUrl")]
	public class StaticResourceUrl : FunctionExpression
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

			// if the resource exist process it otherwise 404
			var version = "0";
			var resourcePath = new RelativeResourcePath(relativePath, false);
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			if (resourceService.Exists(resourcePath))
			{
				// get the resource
				var resource = resourceService.GetSingle(context, resourcePath);

				// get the resource version
				version = resource.Version;
			}

			// create the relative path
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, PathRewriterModule.StaticResourcesPrefix, relativePath);

			// create the uri
			return new Uri(webContext.HttpContext.Request.ApplicationBaseUri, prefixedRelativePath + "?v=" + version);
		}
	}
}