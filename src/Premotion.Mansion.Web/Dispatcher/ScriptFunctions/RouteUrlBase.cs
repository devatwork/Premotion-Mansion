using System;
using System.Linq;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Dispatcher.ScriptFunctions
{
	/// <summary>
	/// Base class for all route url generators.
	/// </summary>
	public abstract class RouteUrlBase : FunctionExpression
	{
		/// <summary>
		/// Assembles the route.
		/// </summary>
		/// <param name="routeParts">The route parts.</param>
		/// <param name="parameterParts">The parameter parts, can be null for no parameters.</param>
		/// <param name="nodeId">The ID if <see cref="NodePointer"/> identified by the assembled route.</param>
		/// <returns>Returns the assembled route.</returns>
		protected static string AssembleRoute(string[] routeParts, string[] parameterParts = null, int nodeId = 0)
		{
			// validate arguments
			if (routeParts == null)
				throw new ArgumentNullException("routeParts");

			// determine the length of the combined route and parameter parts
			// +1 is for Constants.RouteUrlPrefix
			var routePartLength = routeParts.Length + 1;
			// +1 is for Constants.RouteParameterPrefix
			var parameterPartLength = parameterParts == null || parameterParts.Length == 0 ? 0 : parameterParts.Length + 1;

			// construct the url parts
			var urlParts = new string[routePartLength + parameterPartLength];
			urlParts[0] = Constants.RouteUrlPrefix;
			Array.Copy(routeParts, 0, urlParts, 1, routeParts.Length);
			if (parameterParts != null && parameterParts.Length != 0)
			{
				urlParts[routePartLength] = Constants.RouteParameterPrefix;
				Array.Copy(parameterParts, 0, urlParts, routePartLength + 1, parameterParts.Length);
			}

			// add the extension to the last part
			var lastPart = urlParts[urlParts.Length - 1].Trim(Constants.UrlPartTrimCharacters);
			if (nodeId != 0)
				lastPart += "." + nodeId;
			lastPart += "." + Constants.Extension;
			urlParts[urlParts.Length - 1] = lastPart;

			// convert to url
			return string.Join("/", urlParts.Select(part => part.Trim(Constants.UrlPartTrimCharacters)));
		}
	}
}