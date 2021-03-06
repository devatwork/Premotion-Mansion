﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;
using Premotion.Mansion.Web.Hosting.AspNet;

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
			var result = buffer.ToString();
			if (input.Length > 0 && input[0] != '-')
				result = result.TrimStart('-');
			if (input.Length > 0 && input[input.Length - 1] != '-')
				result = result.TrimEnd('-');
			return result;
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
		/// <summary>
		/// Transforms th input text to html.
		/// </summary>
		/// <param name="input">The string which to transform.</param>
		public static string TextToHTML(this string input)
		{
			// check if input is empty
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			var output = new StringBuilder(input.Length);
			var buffer = new StringBuilder(input.Length);
			foreach (var currentCharacter in input)
			{
				buffer.Append(currentCharacter);
				if (currentCharacter != '\n')
					continue;

				// eat line fead and optionally the carriage return
				buffer.Length--;
				if (buffer.Length > 0 && buffer[buffer.Length - 1] == '\r')
					buffer.Length--;

				// turn into a paragraph
				if (buffer.Length == 0)
					output.AppendLine("<p>&nbsp;</p>");
				else
				{
					output.Append("<p>");
					output.Append(buffer.ToString().HtmlEncode());
					output.AppendLine("</p>");
				}
				buffer.Length = 0;
			}

			// empty the remainder
			if (buffer.Length != 0)
			{
				output.Append("<p>");
				output.Append(buffer.ToString().HtmlEncode());
				output.AppendLine("</p>");
			}

			// return the result
			return output.ToString();
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
		/// Gets the <see cref="WebOutputPipe"/> from <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="webOutputPipe">The <see cref="WebOutputPipe"/>.</param>
		/// <returns>Returns the <see cref="WebOutputPipe"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="context"/> does not contain a <see cref="WebOutputPipe"/>.</exception>
		public static bool TryGetWebOuputPipe(this IMansionContext context, out WebOutputPipe webOutputPipe)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// try to find the output pipe
			webOutputPipe = (WebOutputPipe) context.OutputPipeStack.FirstOrDefault(x => x is WebOutputPipe);
			return webOutputPipe != null;
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
			context.SetCookie(new WebCookie {
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

			// write the cookie to the output pipe if there is one, otherwise write it directly to the http response
			WebOutputPipe outputPipe;
			if (context.TryGetWebOuputPipe(out outputPipe))
				outputPipe.Response.Cookies.Add(cookie);
			else
				HttpContextAdapter.TransferCookie(cookie, new HttpResponseWrapper(HttpContext.Current.Response));
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
			return new PropertyBag {
				{"url", url},
				{"path", url.Path},
				{"filename", url.Filename},
				{"basePath", url.BasePath}
			};
		}
		#endregion
		#region ITypeDefinition Extension
		/// <summary>
		/// Gets the label of the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the type definition label.</returns>
		public static string GetTypeDefinitionLabel(this ITypeDefinition type, IMansionContext context)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (context == null)
				throw new ArgumentNullException("context");

			// find the descriptor
			CmsBehaviorDescriptor cmsDescriptor;
			return type.TryFindDescriptorInHierarchy(out cmsDescriptor) ? cmsDescriptor.GetBehavior(context).Label : type.Name;
		}
		#endregion
	}
}