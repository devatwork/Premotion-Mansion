using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Generates a CMS plugin route URL.
	/// </summary>
	[ScriptFunction("CmsNodeBrowserUrl")]
	public class CmsNodeBrowserUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="async">Flag indicating whether to generate a async route or not.</param>
		/// <param name="id">The ID of the node to which to browse.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, bool async, int id, params string[] parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// assemble the plugin parameters
			var pluginParameters = new[] {"CmsBrowserPlugin", "Browse"};

			// assemble the route
			return RouteUrlBuilder.BuildRouteWithArea(context.Cast<IMansionWebContext>(), "Cms", "Cms", async ? "AsyncPlugin" : "Plugin", id, pluginParameters.Concat(parameters).ToArray());
		}
	}
}