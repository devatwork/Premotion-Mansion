using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Url;

namespace Premotion.Mansion.Web.Dispatcher.ScriptFunctions
{
	/// <summary>
	/// Generates a command route URL.
	/// </summary>
	[ScriptFunction("CommandRouteUrl")]
	public class CommandRouteUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, string controller, string action, params string[] parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			return RouteUrlBuilder.BuildRoute(context.Cast<IMansionWebContext>(), controller, action, parameters);
		}
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="area">The name of the area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <param name="parameters">The parameters for the route URL.</param>
		/// <returns>Return the relative URL.</returns>
		public Uri Evaluate(IMansionContext context, string area, string controller, string action, params string[] parameters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(area))
				throw new ArgumentNullException("area");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			return RouteUrlBuilder.BuildRouteWithArea(context.Cast<IMansionWebContext>(), area, controller, action, parameters);
		}
	}
}