using System;
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
		/// /// <param name="area">The area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="area"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRoute(IMansionWebContext context, string area, string controller, string action, params string[] parammeters)
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
		/// <param name="area">The area.</param>
		/// <param name="controller">The name of the controller.</param>
		/// <param name="action">The name of the action.</param>
		/// <param name="nodeId">The node id.</param>
		/// <param name="parammeters">The parameters.</param>
		/// <returns>Returnt the <see cref="Uri"/> for the route.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/>, <paramref name="area"/>, <paramref name="controller"/> or <paramref name="action"/> is null.</exception>
		public static Uri BuildRoute(IMansionWebContext context, string area, string controller, string action, int nodeId, params string[] parammeters)
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
		private static Uri AssembleRoute(IMansionWebContext context, string[] routeParts, string[] parameterParts = null, int nodeId = 0)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (routeParts == null)
				throw new ArgumentNullException("routeParts");

			// determine the length of the combined route and parameter parts
			// +1 is for Constants.RouteUrlPrefix
			var routePartLength = routeParts.Length + 1;
			// +1 is for Constants.RouteParameterPrefix
			var parameterPartLength = parameterParts == null || parameterParts.Length == 0 ? 0 : parameterParts.Length + 1;

			// construct the url parts
			var urlParts = new string[routePartLength + parameterPartLength];
			urlParts[0] = Dispatcher.Constants.RouteUrlPrefix;
			Array.Copy(routeParts, 0, urlParts, 1, routeParts.Length);
			if (parameterParts != null && parameterParts.Length != 0)
			{
				urlParts[routePartLength] = Dispatcher.Constants.RouteParameterPrefix;
				Array.Copy(parameterParts, 0, urlParts, routePartLength + 1, parameterParts.Length);
			}

			// add the extension to the last part
			var lastPart = urlParts[urlParts.Length - 1].Trim(Dispatcher.Constants.UrlPartTrimCharacters);
			if (nodeId != 0)
				lastPart += "." + nodeId;
			lastPart += "." + Dispatcher.Constants.Extension;
			urlParts[urlParts.Length - 1] = lastPart;

			// assemble the relative url
			var relativeUrl = string.Join("/", urlParts.Select(part => part.Trim(Dispatcher.Constants.UrlPartTrimCharacters)));

			// return the uri
			return new Uri(context.ApplicationBaseUri, relativeUrl);
		}
	}
}