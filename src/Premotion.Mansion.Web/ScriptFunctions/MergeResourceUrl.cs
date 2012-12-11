using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a path for merging dynamic resources.
	/// </summary>
	[ScriptFunction("MergeResourceUrl")]
	public class MergeResourceUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a path for dynamic resources.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="relativePath">The relative path to the resource.</param>
		/// <returns>The <see cref="Uri"/> ot the merged resource.</returns>
		public Url Evaluate(IMansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// create the url
			var url = Url.CreateUrl(webContext);

			// create the relative path
			url.PathSegments = WebUtilities.CombineIntoRelativeUrl(MergeResourceRequestHandler.Prefix, relativePath);
			url.CanHaveExtension = true;

			// create the uri
			return url;
		}
	}
}