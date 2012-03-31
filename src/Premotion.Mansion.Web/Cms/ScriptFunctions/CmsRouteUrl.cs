using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Dispatcher.ScriptFunctions;

namespace Premotion.Mansion.Web.Cms.ScriptFunctions
{
	/// <summary>
	/// Generates a route URL.
	/// </summary>
	[ScriptFunction("CmsRouteUrl")]
	public class CmsRouteUrl : RouteUrlBase
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, string controller, string action)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			// assemble the relative url part
			var relativeUrl = AssembleRoute(new[] {"Cms", controller, action});

			// return the uri
			var webContext = context.Cast<IMansionWebContext>();
			return new Uri(webContext.ApplicationBaseUri, relativeUrl);
		}
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <param name="nodeId">The ID of the node.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, string controller, string action, int nodeId, params string[] parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			// assemble the relative url part
			var relativeUrl = AssembleRoute(new[] {"Cms", controller, action}, parameters, nodeId);

			// return the uri
			var webContext = context.Cast<IMansionWebContext>();
			return new Uri(webContext.ApplicationBaseUri, relativeUrl);
		}
	}
}