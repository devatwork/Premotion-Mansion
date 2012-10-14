using System;
using System.Collections.Generic;
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
		private static readonly Regex EmailRegularExpression = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
		#endregion
		#region String Extensions
		/// <summary>
		/// Parses the <paramref name="query"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="query">The query string which to parse.</param>
		/// <returns>Returns the dictionary containing parameter value pairs from the <paramref name="query"/>.</returns>
		public static IPropertyBag ParseQueryString(this string query)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				return new PropertyBag();

			// get the parsed values
			var parsedQueryString = HttpUtility.ParseQueryString(query);

			return parsedQueryString.Cast<string>().Select(x => new KeyValuePair<string, string>(x, parsedQueryString[x])).ToPropertyBag();
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
			return EmailRegularExpression.IsMatch(input);
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
		/// <summary>
		/// Converts this NameValueCollection to a property bag.
		/// </summary>
		/// <param name="collection">The collection which to convert.</param>
		/// <returns>Return the property bag.</returns>
		public static IPropertyBag ToPropertyBag(this IEnumerable<KeyValuePair<string, string>> collection)
		{
			// validate arguments
			if (collection == null)
				throw new ArgumentNullException("collection");

			// extracts needed dataspaces
			return new PropertyBag(collection.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)));
		}
		#endregion
		#region IMansionWebContext Extensions
		/// <summary>
		/// Gets the <see cref="WebOutputPipe"/> from <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="WebOutputPipe"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="context"/> does not contain a <see cref="WebOutputPipe"/>.</exception>
		public static WebOutputPipe GetWebOuputPipe(this IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// try to find the output pipe
			var webOutputPipe = (WebOutputPipe) context.OutputPipeStack.FirstOrDefault(x => x is WebOutputPipe);
			if (webOutputPipe == null)
				throw new InvalidOperationException("No web output pipe was found on the stack.");

			return webOutputPipe;
		}
		/// <summary>
		/// Deletes a cookie from the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cookieName">The name of the cookie which to delete.</param>
		public static void DeleteCookie(this IMansionContext context, string cookieName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(cookieName))
				throw new ArgumentNullException("cookieName");

			// expire the cookie
			context.SetCookie(new WebCookie
			                  {
			                  	Name = cookieName,
			                  	Expires = DateTime.Now.AddDays(-1)
			                  });
		}
		/// <summary>
		/// Sets a cookie in the <paramref name="context"/>
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cookie">The <see cref="WebCookie"/>.</param>
		public static void SetCookie(this IMansionContext context, WebCookie cookie)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (cookie == null)
				throw new ArgumentNullException("cookie");

			// get the output pipe
			var outputPipe = context.GetWebOuputPipe();

			// expire the cookie
			outputPipe.Response.Cookies.Add(cookie);
		}
		#endregion
	}
}