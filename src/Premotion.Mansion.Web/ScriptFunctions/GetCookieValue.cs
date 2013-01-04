using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Gets the value of a cookie.
	/// </summary>
	[ScriptFunction("GetCookieValue")]
	public class GetCookieValue : FunctionExpression
	{
		/// <summary>
		/// Gets the value of a cookie.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cookieName">The string which to encode.</param>
		public string Evaluate(IMansionContext context, string cookieName)
		{
			return Evaluate(context, cookieName, string.Empty);
		}
		/// <summary>
		/// Gets the value of a cookie.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cookieName">The string which to encode.</param>
		/// <param name="defaultValue">The default value.</param>
		public string Evaluate(IMansionContext context, string cookieName, string defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(cookieName))
				throw new ArgumentNullException("context");

			// get the cookie
			WebCookie cookie;
			return context.Cast<IMansionWebContext>().Request.Cookies.TryGetValue(cookieName, out cookie) ? cookie.Value : defaultValue;
		}
	}
}