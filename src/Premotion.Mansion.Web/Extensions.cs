using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
		private static readonly string[][] HtmlNamedEntities = new[]
		                                                       {
		                                                       	new[] {"&quot;", "\""},
		                                                       	new[] {"&lt;", "<"},
		                                                       	new[] {"&gt;", ">"},
		                                                       	new[] {"&nbsp;", " "},
		                                                       	new[] {"&iexcl;", "¡"},
		                                                       	new[] {"&cent;", "¢"},
		                                                       	new[] {"&pound;", "£"},
		                                                       	new[] {"&curren;", "¤"},
		                                                       	new[] {"&yen;", "¥"},
		                                                       	new[] {"&brvbar;", "¦"},
		                                                       	new[] {"&sect;", "§"},
		                                                       	new[] {"&uml;", "¨"},
		                                                       	new[] {"&copy;", "©"},
		                                                       	new[] {"&ordf;", "ª"},
		                                                       	new[] {"&laquo;", "«"},
		                                                       	new[] {"&not;", "¬"},
		                                                       	new[] {"&shy;", "­"},
		                                                       	new[] {"&reg;", "®"},
		                                                       	new[] {"&macr;", "¯"},
		                                                       	new[] {"&deg;", "°"},
		                                                       	new[] {"&plusmn;", "±"},
		                                                       	new[] {"&sup2;", "²"},
		                                                       	new[] {"&sup3;", "³"},
		                                                       	new[] {"&acute;", "´"},
		                                                       	new[] {"&micro;", "µ"},
		                                                       	new[] {"&para;", "¶"},
		                                                       	new[] {"&middot;", "·"},
		                                                       	new[] {"&cedil;", "¸"},
		                                                       	new[] {"&sup1;", "¹"},
		                                                       	new[] {"&ordm;", "º"},
		                                                       	new[] {"&raquo;", " »"},
		                                                       	new[] {"&frac14;", "¼"},
		                                                       	new[] {"&frac12;", "½"},
		                                                       	new[] {"&frac34;", "¾"},
		                                                       	new[] {"&iquest;", "¿"},
		                                                       	new[] {"&Agrave;", "À"},
		                                                       	new[] {"&Aacute;", "Á"},
		                                                       	new[] {"&Acirc;", "Â"},
		                                                       	new[] {"&Atilde;", "Ã"},
		                                                       	new[] {"&Auml;", "Ä"},
		                                                       	new[] {"&Aring;", "Å"},
		                                                       	new[] {"&AElig;", "Æ"},
		                                                       	new[] {"&Ccedil;", "Ç"},
		                                                       	new[] {"&Egrave;", "È"},
		                                                       	new[] {"&Eacute;", "É"},
		                                                       	new[] {"&Ecirc;", "Ê"},
		                                                       	new[] {"&Euml;", "Ë"},
		                                                       	new[] {"&Igrave;", "Ì"},
		                                                       	new[] {"&Iacute;", "Í"},
		                                                       	new[] {"&Icirc;", "Î"},
		                                                       	new[] {"&Iuml;", "Ï"},
		                                                       	new[] {"&ETH;", "Ð"},
		                                                       	new[] {"&Ntilde;", "Ñ"},
		                                                       	new[] {"&Ograve;", "Ò"},
		                                                       	new[] {"&Oacute;", "Ó"},
		                                                       	new[] {"&Ocirc;", "Ô"},
		                                                       	new[] {"&Otilde;", "Õ"},
		                                                       	new[] {"&Ouml;", "Ö"},
		                                                       	new[] {"&times;", "×"},
		                                                       	new[] {"&Oslash;", "Ø"},
		                                                       	new[] {"&Ugrave;", "Ù"},
		                                                       	new[] {"&Uacute;", "Ú"},
		                                                       	new[] {"&Ucirc;", "Û"},
		                                                       	new[] {"&Uuml;", "Ü"},
		                                                       	new[] {"&Yacute;", "Ý"},
		                                                       	new[] {"&THORN;", "Þ"},
		                                                       	new[] {"&szlig;", "ß"},
		                                                       	new[] {"&agrave;", "à"},
		                                                       	new[] {"&aacute;", "á"},
		                                                       	new[] {"&acirc;", "â"},
		                                                       	new[] {"&atilde;", "ã"},
		                                                       	new[] {"&auml;", "ä"},
		                                                       	new[] {"&aring;", "å"},
		                                                       	new[] {"&aelig;", "æ"},
		                                                       	new[] {"&ccedil;", "ç"},
		                                                       	new[] {"&egrave;", "è"},
		                                                       	new[] {"&eacute;", "é"},
		                                                       	new[] {"&ecirc;", "ê"},
		                                                       	new[] {"&euml;", "ë"},
		                                                       	new[] {"&igrave;", "ì"},
		                                                       	new[] {"&iacute;", "í"},
		                                                       	new[] {"&icirc;", "î"},
		                                                       	new[] {"&iuml;", "ï"},
		                                                       	new[] {"&eth;", "ð"},
		                                                       	new[] {"&ntilde;", "ñ"},
		                                                       	new[] {"&ograve;", "ò"},
		                                                       	new[] {"&oacute;", "ó"},
		                                                       	new[] {"&ocirc;", "ô"},
		                                                       	new[] {"&otilde;", "õ"},
		                                                       	new[] {"&ouml;", "ö"},
		                                                       	new[] {"&divide;", "÷"},
		                                                       	new[] {"&oslash;", "ø"},
		                                                       	new[] {"&ugrave;", "ù"},
		                                                       	new[] {"&uacute;", "ú"},
		                                                       	new[] {"&ucirc;", "û"},
		                                                       	new[] {"&uuml;", "ü"},
		                                                       	new[] {"&yacute;", "ý"},
		                                                       	new[] {"&thorn;", "þ"},
		                                                       	new[] {"&yuml;", "ÿ"},
		                                                       	new[] {"&OElig;", "Œ"},
		                                                       	new[] {"&oelig;", "œ"},
		                                                       	new[] {"&Scaron;", "Š"},
		                                                       	new[] {"&scaron;", "š"},
		                                                       	new[] {"&Yuml;", "Ÿ"},
		                                                       	new[] {"&fnof;", "ƒ"},
		                                                       	new[] {"&circ;", "ˆ"},
		                                                       	new[] {"&tilde;", "˜"},
		                                                       	new[] {"&Alpha;", "Α"},
		                                                       	new[] {"&Beta;", "Β"},
		                                                       	new[] {"&Gamma;", "Γ"},
		                                                       	new[] {"&Delta;", "Δ"},
		                                                       	new[] {"&Epsilon;", "Ε"},
		                                                       	new[] {"&Zeta;", "Ζ"},
		                                                       	new[] {"&Eta;", "Η"},
		                                                       	new[] {"&Theta;", "Θ"},
		                                                       	new[] {"&Iota;", "Ι"},
		                                                       	new[] {"&Kappa;", "Κ"},
		                                                       	new[] {"&Lambda;", "Λ"},
		                                                       	new[] {"&Mu;", "Μ"},
		                                                       	new[] {"&Nu;", "Ν"},
		                                                       	new[] {"&Xi;", "Ξ"},
		                                                       	new[] {"&Omicron;", "Ο"},
		                                                       	new[] {"&Pi;", "Π"},
		                                                       	new[] {"&Rho;", "Ρ"},
		                                                       	new[] {"&Sigma;", "Σ"},
		                                                       	new[] {"&Tau;", "Τ"},
		                                                       	new[] {"&Upsilon;", "Υ"},
		                                                       	new[] {"&Phi;", "Φ"},
		                                                       	new[] {"&Chi;", "Χ"},
		                                                       	new[] {"&Psi;", "Ψ"},
		                                                       	new[] {"&Omega;", "Ω"},
		                                                       	new[] {"&alpha;", "α"},
		                                                       	new[] {"&beta;", "β"},
		                                                       	new[] {"&gamma;", "γ"},
		                                                       	new[] {"&delta;", "δ"},
		                                                       	new[] {"&epsilon;", "ε"},
		                                                       	new[] {"&zeta;", "ζ"},
		                                                       	new[] {"&eta;", "η"},
		                                                       	new[] {"&theta;", "θ"},
		                                                       	new[] {"&iota;", "ι"},
		                                                       	new[] {"&kappa;", "κ"},
		                                                       	new[] {"&lambda;", "λ"},
		                                                       	new[] {"&mu;", "μ"},
		                                                       	new[] {"&nu;", "ν"},
		                                                       	new[] {"&xi;", "ξ"},
		                                                       	new[] {"&omicron;", "ο"},
		                                                       	new[] {"&pi;", "π"},
		                                                       	new[] {"&rho;", "ρ"},
		                                                       	new[] {"&sigmaf;", "ς"},
		                                                       	new[] {"&sigma;", "σ"},
		                                                       	new[] {"&tau;", "τ"},
		                                                       	new[] {"&upsilon;", "υ"},
		                                                       	new[] {"&phi;", "φ"},
		                                                       	new[] {"&chi;", "χ"},
		                                                       	new[] {"&psi;", "ψ"},
		                                                       	new[] {"&omega;", "ω"},
		                                                       	new[] {"&thetasym;", "ϑ"},
		                                                       	new[] {"&upsih;", "ϒ"},
		                                                       	new[] {"&piv;", "ϖ"},
		                                                       	new[] {"&ensp;", " "},
		                                                       	new[] {"&emsp;", " "},
		                                                       	new[] {"&thinsp;", " "},
		                                                       	new[] {"&zwnj;", "‌"},
		                                                       	new[] {"&zwj;", "‍"},
		                                                       	new[] {"&lrm;", "‎"},
		                                                       	new[] {"&rlm;", "‏"},
		                                                       	new[] {"&ndash;", "–"},
		                                                       	new[] {"&mdash;", "—"},
		                                                       	new[] {"&lsquo;", "‘"},
		                                                       	new[] {"&rsquo;", "’"},
		                                                       	new[] {"&sbquo;", "‚"},
		                                                       	new[] {"&ldquo;", "“"},
		                                                       	new[] {"&rdquo;", "”"},
		                                                       	new[] {"&bdquo;", "„"},
		                                                       	new[] {"&dagger;", "†"},
		                                                       	new[] {"&Dagger;", "‡"},
		                                                       	new[] {"&bull;", "•"},
		                                                       	new[] {"&hellip;", "…"},
		                                                       	new[] {"&permil;", "‰"},
		                                                       	new[] {"&prime;", "′"},
		                                                       	new[] {"&Prime;", "″"},
		                                                       	new[] {"&lsaquo;", "‹"},
		                                                       	new[] {"&rsaquo;", "›"},
		                                                       	new[] {"&oline;", "‾"},
		                                                       	new[] {"&frasl;", "⁄"},
		                                                       	new[] {"&euro;", "€"},
		                                                       	new[] {"&image;", "ℑ"},
		                                                       	new[] {"&weierp;", "℘"},
		                                                       	new[] {"&real;", "ℜ"},
		                                                       	new[] {"&trade;", "™"},
		                                                       	new[] {"&alefsym;", "ℵ"},
		                                                       	new[] {"&larr;", "←"},
		                                                       	new[] {"&uarr;", "↑"},
		                                                       	new[] {"&rarr;", "→"},
		                                                       	new[] {"&darr;", "↓"},
		                                                       	new[] {"&harr;", "↔"},
		                                                       	new[] {"&crarr;", "↵"},
		                                                       	new[] {"&lArr;", "⇐"},
		                                                       	new[] {"&uArr;", "⇑"},
		                                                       	new[] {"&rArr;", "⇒"},
		                                                       	new[] {"&dArr;", "⇓"},
		                                                       	new[] {"&hArr;", "⇔"},
		                                                       	new[] {"&forall;", "∀"},
		                                                       	new[] {"&part;", "∂"},
		                                                       	new[] {"&exist;", "∃"},
		                                                       	new[] {"&empty;", "∅"},
		                                                       	new[] {"&nabla;", "∇"},
		                                                       	new[] {"&isin;", "∈"},
		                                                       	new[] {"&notin;", "∉"},
		                                                       	new[] {"&ni;", "∋"},
		                                                       	new[] {"&prod;", "∏"},
		                                                       	new[] {"&sum;", "∑"},
		                                                       	new[] {"&minus;", "−"},
		                                                       	new[] {"&lowast;", "∗"},
		                                                       	new[] {"&radic;", "√"},
		                                                       	new[] {"&prop;", "∝"},
		                                                       	new[] {"&infin;", "∞"},
		                                                       	new[] {"&ang;", "∠"},
		                                                       	new[] {"&and;", "∧"},
		                                                       	new[] {"&or;", "∨"},
		                                                       	new[] {"&cap;", "∩"},
		                                                       	new[] {"&cup;", "∪"},
		                                                       	new[] {"&int;", "∫"},
		                                                       	new[] {"&there4;", "∴"},
		                                                       	new[] {"&sim;", "∼"},
		                                                       	new[] {"&cong;", "≅"},
		                                                       	new[] {"&asymp;", "≈"},
		                                                       	new[] {"&ne;", "≠"},
		                                                       	new[] {"&equiv;", "≡"},
		                                                       	new[] {"&le;", "≤"},
		                                                       	new[] {"&ge;", "≥"},
		                                                       	new[] {"&sub;", "⊂"},
		                                                       	new[] {"&sup;", "⊃"},
		                                                       	new[] {"&nsub;", "⊄"},
		                                                       	new[] {"&sube;", "⊆"},
		                                                       	new[] {"&supe;", "⊇"},
		                                                       	new[] {"&oplus;", "⊕"},
		                                                       	new[] {"&otimes;", "⊗"},
		                                                       	new[] {"&perp;", "⊥"},
		                                                       	new[] {"&sdot;", "⋅"},
		                                                       	new[] {"&lceil;", "⌈"},
		                                                       	new[] {"&rceil;", "⌉"},
		                                                       	new[] {"&lfloor;", "⌊"},
		                                                       	new[] {"&rfloor;", "⌋"},
		                                                       	new[] {"&lang;", "〈"},
		                                                       	new[] {"&rang;", "〉"},
		                                                       	new[] {"&loz;", "◊"},
		                                                       	new[] {"&spades;", "♠"},
		                                                       	new[] {"&clubs;", "♣"},
		                                                       	new[] {"&hearts;", "♥"},
		                                                       	new[] {"&diams;", "♦"},
		                                                       	new[] {"&amp;", "&"}
		                                                       };
		#endregion
		#region IHttpContext Extensions
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
		/// Strips the HTML from the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to strip.</param>
		/// <returns>Returns the stripped <paramref name="input"/>.</returns>
		/// <remarks>http://www.codeproject.com/KB/MCMS/htmlTagStripper.aspx</remarks>
		public static string StripHtml(this string input)
		{
			if (input == null)
				return null;
			input = input.Trim();
			if (input == string.Empty)
				return string.Empty;

			var bodyStartTagIdx = input.IndexOf("<body", StringComparison.CurrentCultureIgnoreCase);
			var bodyEndTagIdx = input.IndexOf("</body>", StringComparison.CurrentCultureIgnoreCase);

			int startIdx = 0, endIdx = input.Length - 1;
			if (bodyStartTagIdx >= 0)
				startIdx = bodyStartTagIdx;
			if (bodyEndTagIdx >= 0)
				endIdx = bodyEndTagIdx;

			bool insideTag = false,
			     insideAttributeValue = false,
			     insideHtmlComment = false,
			     insideScriptBlock = false,
			     insideNoScriptBlock = false,
			     insideStyleBlock = false;
			var attributeValueDelimiter = '"';

			var sb = new StringBuilder(input.Length);
			for (var i = startIdx; i <= endIdx; i++)
			{
				// html comment block
				if (!insideHtmlComment)
				{
					if (i + 3 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '!' &&
					    input[i + 2] == '-' &&
					    input[i + 3] == '-')
					{
						i += 3;
						insideHtmlComment = true;
						continue;
					}
				}
				else // inside html comment
				{
					if (i + 2 < input.Length &&
					    input[i] == '-' &&
					    input[i + 1] == '-' &&
					    input[i + 2] == '>')
					{
						i += 2;
						insideHtmlComment = false;
					}
					continue;
				}

				// noscript block
				if (!insideNoScriptBlock)
				{
					if (i + 9 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 'n' || input[i + 1] == 'N') &&
					    (input[i + 2] == 'o' || input[i + 2] == 'O') &&
					    (input[i + 3] == 's' || input[i + 3] == 'S') &&
					    (input[i + 4] == 'c' || input[i + 4] == 'C') &&
					    (input[i + 5] == 'r' || input[i + 5] == 'R') &&
					    (input[i + 6] == 'i' || input[i + 6] == 'I') &&
					    (input[i + 7] == 'p' || input[i + 7] == 'P') &&
					    (input[i + 8] == 't' || input[i + 8] == 'T') &&
					    (char.IsWhiteSpace(input[i + 9]) || input[i + 9] == '>'))
					{
						i += 9;
						insideNoScriptBlock = true;
						continue;
					}
				}
				else // inside noscript block
				{
					if (i + 10 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 'n' || input[i + 2] == 'N') &&
					    (input[i + 3] == 'o' || input[i + 3] == 'O') &&
					    (input[i + 4] == 's' || input[i + 4] == 'S') &&
					    (input[i + 5] == 'c' || input[i + 5] == 'C') &&
					    (input[i + 6] == 'r' || input[i + 6] == 'R') &&
					    (input[i + 7] == 'i' || input[i + 7] == 'I') &&
					    (input[i + 8] == 'p' || input[i + 8] == 'P') &&
					    (input[i + 9] == 't' || input[i + 9] == 'T') &&
					    (char.IsWhiteSpace(input[i + 10]) || input[i + 10] == '>'))
					{
						if (input[i + 10] != '>')
						{
							i += 9;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 10;
						insideNoScriptBlock = false;
					}
					continue;
				}

				// script block
				if (!insideScriptBlock)
				{
					if (i + 7 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 's' || input[i + 1] == 'S') &&
					    (input[i + 2] == 'c' || input[i + 2] == 'C') &&
					    (input[i + 3] == 'r' || input[i + 3] == 'R') &&
					    (input[i + 4] == 'i' || input[i + 4] == 'I') &&
					    (input[i + 5] == 'p' || input[i + 5] == 'P') &&
					    (input[i + 6] == 't' || input[i + 6] == 'T') &&
					    (char.IsWhiteSpace(input[i + 7]) || input[i + 7] == '>'))
					{
						i += 6;
						insideScriptBlock = true;
						continue;
					}
				}
				else // inside script block
				{
					if (i + 8 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 's' || input[i + 2] == 'S') &&
					    (input[i + 3] == 'c' || input[i + 3] == 'C') &&
					    (input[i + 4] == 'r' || input[i + 4] == 'R') &&
					    (input[i + 5] == 'i' || input[i + 5] == 'I') &&
					    (input[i + 6] == 'p' || input[i + 6] == 'P') &&
					    (input[i + 7] == 't' || input[i + 7] == 'T') &&
					    (char.IsWhiteSpace(input[i + 8]) || input[i + 8] == '>'))
					{
						if (input[i + 8] != '>')
						{
							i += 7;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 8;
						insideScriptBlock = false;
					}
					continue;
				}

				// style block
				if (!insideStyleBlock)
				{
					if (i + 7 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 's' || input[i + 1] == 'S') &&
					    (input[i + 2] == 't' || input[i + 2] == 'T') &&
					    (input[i + 3] == 'y' || input[i + 3] == 'Y') &&
					    (input[i + 4] == 'l' || input[i + 4] == 'L') &&
					    (input[i + 5] == 'e' || input[i + 5] == 'E') &&
					    (char.IsWhiteSpace(input[i + 6]) || input[i + 6] == '>'))
					{
						i += 5;
						insideStyleBlock = true;
						continue;
					}
				}
				else // inside script block
				{
					if (i + 8 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 's' || input[i + 2] == 'S') &&
					    (input[i + 3] == 't' || input[i + 3] == 'C') &&
					    (input[i + 4] == 'y' || input[i + 4] == 'R') &&
					    (input[i + 5] == 'l' || input[i + 5] == 'I') &&
					    (input[i + 6] == 'e' || input[i + 6] == 'P') &&
					    (char.IsWhiteSpace(input[i + 7]) || input[i + 7] == '>'))
					{
						if (input[i + 7] != '>')
						{
							i += 7;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 7;
						insideStyleBlock = false;
					}
					continue;
				}

				if (!insideTag)
				{
					if (i < input.Length &&
					    input[i] == '<')
					{
						insideTag = true;
						continue;
					}
				}
				else // inside tag
				{
					if (!insideAttributeValue)
					{
						if (input[i] == '"' || input[i] == '\'')
						{
							attributeValueDelimiter = input[i];
							insideAttributeValue = true;
							continue;
						}
						if (input[i] == '>')
						{
							insideTag = false;
							sb.Append(' '); // prevent words from different tags (<td>s for example) from joining together
							continue;
						}
					}
					else // inside tag and inside attribute value
					{
						if (input[i] == attributeValueDelimiter)
						{
							insideAttributeValue = false;
							continue;
						}
					}
					continue;
				}

				sb.Append(input[i]);
			}

			foreach (var htmlNamedEntity in HtmlNamedEntities)
				sb.Replace(htmlNamedEntity[0], htmlNamedEntity[1]);

			for (var i = 0; i < 512; i++)
				sb.Replace("&#" + i + ";", ((char) i).ToString());

			return sb.ToString();
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