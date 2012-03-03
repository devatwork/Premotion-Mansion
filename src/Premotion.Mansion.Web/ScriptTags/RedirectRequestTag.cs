using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Redirects the client to another URL.
	/// </summary>
	[Named(Constants.NamespaceUri, "redirectRequest")]
	public class RedirectRequestTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			WebUtilities.RedirectRequest(context, GetRequiredAttribute<Uri>(context, "url"), GetAttribute<bool>(context, "permanent"));
		}
		#endregion
	}
}