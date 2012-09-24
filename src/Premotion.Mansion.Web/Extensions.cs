using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Implements extensions.
	/// </summary>
	public static class Extensions
	{
		#region Constants
		/// <summary>
		/// RFC 2822, http://www.regular-expressions.info/email.html
		/// </summary>
		private static readonly Regex emailRegularExpression = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
		#endregion
		#region HttpContextBase Extensions
		/// <summary>
		/// Gets a flag indicating whether the <paramref name="httpContext"/> has a session.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContextBase"/>.</param>
		/// <returns>Returns true when the <paramref name="httpContext"/> has a sessions, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="httpContext"/> is null.</exception>
		public static bool HasSession(this HttpContextBase httpContext)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			return httpContext.Session != null;
		}
		/// <summary>
		/// Deletes a cookie from the <see cref="HttpContext"/>.
		/// </summary>
		/// <param name="httpContext">The http context.</param>
		/// <param name="cookieName">The name of the cookie which to delete.</param>
		public static void DeleteCookie(this HttpContextBase httpContext, string cookieName)
		{
			// validate arguments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");
			if (string.IsNullOrEmpty(cookieName))
				throw new ArgumentNullException("cookieName");

			// expire the cookie
			httpContext.Response.SetCookie(new HttpCookie(cookieName)
			                               {
			                               	Expires = DateTime.Now.AddDays(-1)
			                               });
		}
		#endregion
		#region HttpRequestBase Extensions
		/// <summary>
		/// Gets the path of the request without the handler prefix.
		/// </summary>
		/// <param name="request">The <see cref="HttpRequestBase"/>.</param>
		/// <returns>Returns the path.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		public static string GetPathWithoutHandlerPrefix(this HttpRequestBase request)
		{
			// validate arguments
			if (request == null)
				throw new ArgumentNullException("request");

			// get the raw uri
			var path = request.Path.Substring(request.ApplicationPath.Length);

			// split the path
			var pathParts = path.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);
			if (pathParts.Length < 1)
				return path;

			// rejoin without the first part
			return String.Join("/", pathParts, 1, pathParts.Length - 1);
		}
		#endregion
		#region NameValueCollection Extensions
		/// <summary>
		/// Converts this NameValueCollection to a property bag.
		/// </summary>
		/// <param name="collection">The collection which to convert.</param>
		/// <returns>Return the property bag.</returns>
		public static IPropertyBag ToPropertyBag(this NameValueCollection collection)
		{
			// validate arguments
			if (collection == null)
				throw new ArgumentNullException("collection");

			// extracts needed dataspaces
			var bag = new PropertyBag();
			foreach (var key in collection.AllKeys.Where(candidate => !string.IsNullOrWhiteSpace(candidate)))
				bag.Set(key, collection[key]);

			return bag;
		}
		#endregion
		#region String Extensions
		/// <summary>
		/// Parses the <paramref name="query"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="query">The query string which to parse.</param>
		/// <returns>Returns the dictionary containing parameter value pairs from the <paramref name="query"/>.</returns>
		public static IPropertyBag ParseQueryString(this string query)
		{
			return string.IsNullOrEmpty(query) ? new PropertyBag() : HttpUtility.ParseQueryString(query).ToPropertyBag();
		}
		/// <summary>
		/// URL encodes the <paramref name="input"/> string.
		/// </summary>
		/// <param name="input">The string which to encode.</param>
		/// <returns>Returns the encoded string.</returns>
		public static string UrlEncode(this string input)
		{
			// validate arguments
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			return HttpUtility.UrlEncode(input);
		}
		/// <summary>
		/// HTML encodes the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to encode.</param>
		/// <returns>Returns the encoded <paramref name="input"/>.</returns>
		public static string HtmlEncode(this string input)
		{
			// validate arguments
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// encode spaces
			var spaces = input.Replace(" ", "%20");

			return HttpUtility.HtmlEncode(spaces);
		}
		/// <summary>
		/// HTML decodes the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to decode.</param>
		/// <returns>Returns the decoded <paramref name="input"/>.</returns>
		public static string HtmlDecode(this string input)
		{
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.HtmlDecode(input);
		}
		/// <summary>
		/// Checks wether the given <paramref name="input"/> is a valid emailaddress or not.
		/// </summary>
		/// <param name="input">The input which to check.</param>
		/// <returns>Returns true if the <paramref name="input"/> is a valid emailaddress, otherwise false.</returns>
		public static bool IsValidEmailAddress(this string input)
		{
			// guard
			if (string.IsNullOrEmpty(input))
				return false;

			// check
			return emailRegularExpression.IsMatch(input);
		}
		#endregion
		#region IPropertyBag Extensions
		/// <summary>
		/// Parses the <paramref name="properties"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="properties">The query string which to parse.</param>
		/// <returns>Returns the dictionary containing parameter value pairs from the <paramref name="properties"/>.</returns>
		public static string ToHttpSafeString(this IEnumerable<KeyValuePair<string, object>> properties)
		{
			// validate arguments
			if (properties == null)
				throw new ArgumentNullException("properties");

			return string.Join("&", properties.Where(property => property.Value != null).Select(property => property.Key.UrlEncode() + "=" + property.Value.ToString().UrlEncode()).ToArray());
		}
		#endregion
	}
}