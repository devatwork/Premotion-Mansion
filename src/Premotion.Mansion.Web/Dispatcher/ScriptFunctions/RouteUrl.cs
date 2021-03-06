﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Urls;

namespace Premotion.Mansion.Web.Dispatcher.ScriptFunctions
{
	/// <summary>
	/// Generates a route URL.
	/// </summary>
	[ScriptFunction("RouteUrl")]
	public class RouteUrl : FunctionExpression
	{
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <returns>Return the relative URL.</returns>
		public Url Evaluate(IMansionContext context, string controller, string action)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			return RouteUrlBuilder.BuildRoute(context.Cast<IMansionWebContext>(), controller, action);
		}
		/// <summary>
		/// Generates a route URL.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The action of the controller.</param>
		/// <param name="nodeId">The ID of the node.</param>
		/// <returns>Return the relative URL.</returns>
		public Url Evaluate(IMansionContext context, string controller, string action, int nodeId)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			return RouteUrlBuilder.BuildRoute(context.Cast<IMansionWebContext>(), controller, action, nodeId);
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
		public Url Evaluate(IMansionContext context, string controller, string action, int nodeId, params string[] parameters)
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

			return RouteUrlBuilder.BuildRoute(context.Cast<IMansionWebContext>(), controller, action, nodeId, parameters);
		}
	}
}