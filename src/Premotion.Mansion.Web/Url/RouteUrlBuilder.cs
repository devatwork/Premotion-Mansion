using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Url
{
	/// <summary>
	/// Builds routes.
	/// </summary>
	public static class RouteUrlBuilder
	{
		/// <summary>
		/// Constructs a route <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRoute(IMansionWebContext context, string controller, string action, params string[] parammeters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			return AssembleRoute(context, new[] {controller, action}, parammeters);
		}
		/// <summary>
		/// Constructs a route <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="nodeId">The node id.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRoute(IMansionWebContext context, string controller, string action, int nodeId, params string[] parammeters)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(controller))
				throw new ArgumentNullException("controller");
			if (string.IsNullOrEmpty(action))
				throw new ArgumentNullException("action");

			return AssembleRoute(context, new[] {controller, action}, parammeters, nodeId);
		}
		/// <summary>
		/// Constructs a route <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// /// <param name="area">The area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="area"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRouteWithArea(IMansionWebContext context, string area, string controller, string action, params string[] parammeters)
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
			return AssembleRoute(context, new[] {area, controller, action}, parammeters);
		}
		/// <summary>
		/// Constructs a route <see cref="Uri"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="area">The area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="nodeId">The node id.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="area"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRouteWithArea(IMansionWebContext context, string area, string controller, string action, int nodeId, params string[] parammeters)
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

			return AssembleRoute(context, new[] {area, controller, action}, parammeters, nodeId);
		}
		/// <summary>
		/// Assembles the route.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="routeParts">The route parts.</param>
		/// <param name="parameterParts">The parameter parts, can be null for no parameters.</param>
		/// <param name="nodeId">The ID if <see cref="NodePointer"/> identified by the assembled route.</param>
		/// <returns>Returns the assembled route.</returns>
		private static Uri AssembleRoute(IMansionWebContext context, IEnumerable<string> routeParts, string[] parameterParts = null, int nodeId = 0)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (routeParts == null)
				throw new ArgumentNullException("routeParts");

			// construct the url parts
			var urlParts = new List<string>();

			// check if there is an ID
			if (nodeId != 0)
				urlParts.Add(nodeId.ToString());

			// add the route url prefix
			urlParts.Add(Dispatcher.Constants.RouteUrlPrefix);

			// add the route parts
			urlParts.AddRange(routeParts);

			// add the parameter parts if any
			if (parameterParts != null && parameterParts.Length != 0)
			{
				// add the parameter prefix
				urlParts.Add(Dispatcher.Constants.RouteParameterPrefix);

				//  add the parameter parts
				urlParts.AddRange(parameterParts);
			}

			// assemble the relative url
			var relativeUrl = string.Join("/", urlParts.Select(part => part.Trim(Dispatcher.Constants.UrlPartTrimCharacters)));

			// return the uri
			return new Uri(context.ApplicationBaseUri, relativeUrl);
		}
	}
}