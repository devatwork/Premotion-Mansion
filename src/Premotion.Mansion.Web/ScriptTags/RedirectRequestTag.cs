using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Redirects the client to another URL.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "redirectRequest")]
	public class RedirectRequestTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			WebUtilities.RedirectRequest(context, GetRequiredAttribute<Uri>(context, "url"), GetAttribute<bool>(context, "permanent"));
		}
		#endregion
	}
}