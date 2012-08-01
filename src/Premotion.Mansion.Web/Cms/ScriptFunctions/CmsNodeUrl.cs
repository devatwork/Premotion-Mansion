using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Generates a CMS route URL.
	/// </summary>
	[ScriptFunction("CmsNodeUrl")]
	public class CmsNodeUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="nodeId">The ID of the node.</param>
		/// <param name="pluginType">The name of the plugin type.</param>
		/// <param name="viewName">The name of the view which to render.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, int nodeId, string pluginType, string viewName, params string[] parameters)
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

			// assemble the plugin type and view
			var pluginParameters = new[] {pluginType, viewName};

			return RouteUrlBuilder.BuildBackofficeRouteWithArea(context.Cast<IMansionWebContext>(), "Cms", "Cms", "Plugin", nodeId, pluginParameters.Concat(parameters).ToArray());
		}
	}
}