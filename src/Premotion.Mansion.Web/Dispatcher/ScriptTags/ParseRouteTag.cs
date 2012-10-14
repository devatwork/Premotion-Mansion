using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Dispatcher.ScriptTags
{
	/// <summary>
	/// Parses a route.
	/// </summary>
	[ScriptTag(Constants.TagNamespaceUri, "parseRoute")]
	public class ParseRouteTag : GetRowBaseTag
	{
		/// <summary>
		/// Gets the row.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="attributes">The attributes of this tag.</param>
		/// <returns>Returns the result.</returns>
		protected override IPropertyBag Get(IMansionContext context, IPropertyBag attributes)
		{
			// get the web request context
			var webRequest = context.Cast<IMansionWebContext>();

			// get the url
			Url url;
			if (!attributes.TryGet(context, "url", out url))
				url = webRequest.Request.RequestUrl;

			// get the area prefix
			var routeProperties = new PropertyBag();
			var routeUrlIndex = Array.FindIndex(url.PathSegments, x => x.Equals(Constants.RouteUrlPrefix, StringComparison.OrdinalIgnoreCase));

			// check if this is no route url
			if (routeUrlIndex == -1)
			{
				// this is no route URL, default controller and action
				routeProperties.Set("area", GetAttribute<string>(context, "defaultArea"));
				routeProperties.Set("controller", GetAttribute<string>(context, "defaultController"));
				routeProperties.Set("action", GetAttribute<string>(context, "defaultAction"));
				return routeProperties;
			}

			// determine the number of route url parts
			var parameterRouteUrlIndex = Array.FindIndex(url.PathSegments, x => x.Equals(Constants.RouteParameterPrefix, StringComparison.OrdinalIgnoreCase));
			var routeUrlPartLength = (parameterRouteUrlIndex != -1 ? parameterRouteUrlIndex : url.PathSegments.Length) - routeUrlIndex;

			// parse the route parts
			if (routeUrlIndex != -1 && routeUrlPartLength == 4)
			{
				// route url with area
				routeProperties.Set("area", GetSegment(url, routeUrlIndex + 1));
				routeProperties.Set("controller", GetSegment(url, routeUrlIndex + 2));
				routeProperties.Set("action", GetSegment(url, routeUrlIndex + 3));
			}
			else if (routeUrlIndex != -1 && routeUrlPartLength == 3)
			{
				// route url without area
				routeProperties.Set("controller", GetSegment(url, routeUrlIndex + 1));
				routeProperties.Set("action", GetSegment(url, routeUrlIndex + 2));
			}
			else
			{
				// unknown route type
				throw new InvalidOperationException(string.Format("'{0}' is an invalid route url", url));
			}

			// parse parameters if any
			if (parameterRouteUrlIndex > -1)
			{
				// set all the parameters
				for (var paremeterIndex = parameterRouteUrlIndex + 1; paremeterIndex < url.PathSegments.Length; paremeterIndex++)
					routeProperties.Set("routeParameter" + ((paremeterIndex - parameterRouteUrlIndex) - 1), GetSegment(url, paremeterIndex));
			}

			// return the route
			return routeProperties;
		}
		/// <summary>
		/// Gets and trims the segment a the <paramref name="index"/>.
		/// </summary>
		/// <param name="url">The <see cref="Uri"/> from which to get the segment.</param>
		/// <param name="index">The index of the segment which to get.</param>
		/// <returns>Returns the trimmed segment.</returns>
		private static string GetSegment(Url url, int index)
		{
			// validate arguments
			if (url == null)
				throw new ArgumentNullException("url");
			if (index < 0 || index >= url.PathSegments.Length)
				throw new ArgumentOutOfRangeException("index");

			// trim the segment
			var segment = url.PathSegments[index];
			segment = segment.Trim(Constants.UrlPartTrimCharacters);
			return segment;
		}
	}
}