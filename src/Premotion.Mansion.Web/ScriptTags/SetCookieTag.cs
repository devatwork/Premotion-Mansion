using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Sets a cookie.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "setCookie")]
	public class SetCookieTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// create the cookie
			var cookie = new WebCookie {
				Name = GetRequiredAttribute<string>(context, "name"),
				HttpOnly = GetAttribute(context, "httpOnly", true),
				Expires = GetAttribute(context, "expires", DateTime.Today.AddDays(30)),
				Value = GetRequiredAttribute<string>(context, "value")
			};

			//  set the cookie
			context.SetCookie(cookie);
		}
		#endregion
	}
}