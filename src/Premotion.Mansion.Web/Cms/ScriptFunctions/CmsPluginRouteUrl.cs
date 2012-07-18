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
	[ScriptFunction("CmsPluginRouteUrl")]
	public class CmsPluginRouteUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pluginType">The type name of the plugin.</param>
		/// <param name="viewName">The view name of the plugin.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, string pluginType, string viewName, params string[] parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(pluginType))
				throw new ArgumentNullException("pluginType");
			if (string.IsNullOrEmpty(viewName))
				throw new ArgumentNullException("viewName");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// assemble the plugin parameters
			var pluginParameters = new[] {pluginType, viewName};

			// assemble the route
			return RouteUrlBuilder.BuildRouteWithArea(context.Cast<IMansionWebContext>(), "Cms", "Cms", "Plugin", pluginParameters.Concat(parameters).ToArray());
		}
	}
}