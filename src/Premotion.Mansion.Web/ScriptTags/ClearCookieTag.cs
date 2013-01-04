using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Clears a cookie.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "clearCookie")]
	public class ClearCookieTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// clear the cookie
			context.DeleteCookie(GetRequiredAttribute<string>(context, "name"));
		}
		#endregion
	}
}