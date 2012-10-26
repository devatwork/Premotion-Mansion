using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		/// <summary>
		/// Unreserved URL path characters.
		/// </summary>
		private static readonly char[] UnreservedUrlPathEncodingCharacters = new[] {'-', '_', '!', '~', '*', '\'', '(', ')'};
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
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.UrlEncode(input);
		}
		/// <summary>
		/// URL path encodes the <paramref name="input"/> string.
		/// </summary>
		/// <param name="input">The string which to encode.</param>
		/// <param name="hasExtension">Flag indicating whether this path can contain an extension</param>
		/// <returns>Returns the path encoded string.</returns>
		public static string UrlPathEncode(this string input, bool hasExtension = false)
		{
			// validate arugments
			if (input == null)
				throw new ArgumentNullException("input");

			// normalize the string
			var normalized = input.Normalize(NormalizationForm.FormKD);
			var removal = Encoding.GetEncoding(Encoding.ASCII.CodePage, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
			var bytes = removal.GetBytes(normalized);
			normalized = Encoding.ASCII.GetString(bytes);

			// remove all spaces and replace them
			var buffer = new StringBuilder(normalized.Length);
			var previousCharacter = 'a';
			foreach (var currentChar in normalized)
			{
				if (Char.IsLetterOrDigit(currentChar) || UnreservedUrlPathEncodingCharacters.Contains(currentChar) || (currentChar == '.' && hasExtension))
				{
					previousCharacter = currentChar;
					buffer.Append(currentChar);
					continue;
				}

				// check if the previous character was an non spacing character
				if (Char.IsLetterOrDigit(previousCharacter))
					buffer.Append('-');

				// ignore this character and move on
				previousCharacter = currentChar;
			}
			return buffer.ToString().Trim('-');
		}
		/// <summary>
		/// URL decodes the <paramref name="input"/> string.
		/// </summary>
		/// <param name="input">The string which to decode.</param>
		/// <returns>Returns the decoded string.</returns>
		public static string UrlDecode(this string input)
		{
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.UrlDecode(input);
		}
		/// <summary>
		/// HTML encodes the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to encode.</param>
		/// <returns>Returns the encoded <paramref name="input"/>.</returns>
		public static string HtmlEncode(this string input)
		{
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.HtmlEncode(input);
		}
		/// <summary>
		/// HTML attribute encodes the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to encode.</param>
		/// <returns>Returns the encoded <paramref name="input"/>.</returns>
		public static string HtmlAttributeEncode(this string input)
		{
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.HtmlAttributeEncode(input);
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
		/// JavaScript encodes the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to encode.</param>
		/// <returns>Returns the encoded <paramref name="input"/>.</returns>
		public static string JavaScriptStringEncode(this string input)
		{
			return string.IsNullOrEmpty(input) ? string.Empty : HttpUtility.JavaScriptStringEncode(input);
		}
		/// <summary>
		/// Checks wether the given <paramref name="input"/> is a valid emailaddress or not.
		/// </summary>
		/// <param name="input">The input which to check.</param>
		/// <returns>Returns true if the <paramref name="input"/> is a valid emailaddress, otherwise false.</returns>
		public static bool IsValidEmailAddress(this string input)
		{
			return !string.IsNullOrEmpty(input) && EmailRegularExpression.IsMatch(input);
		}
		#endregion
		#region IPropertyBag Extensions
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
		#region Url Extensions
		/// <summary>
		/// Converts the given <paramref name="url"/> to a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="url">The <see cref="Url"/> which to convert.</param>
		/// <returns>Returns the converted <see cref="IPropertyBag"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="url"/> is null.</exception>
		public static IPropertyBag ToPropertyBag(this Url url)
		{
			// validate arguments
			if (url == null)
				throw new ArgumentNullException("url");

			// create the property bag
			return new PropertyBag
			       {
			       	{"url", url},
			       	{"path", url.Path},
			       	{"filename", url.Filename},
			       	{"basePath", url.BasePath}
			       };
		}
		#endregion
	}
}