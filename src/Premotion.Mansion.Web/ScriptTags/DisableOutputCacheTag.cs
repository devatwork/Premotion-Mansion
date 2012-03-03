using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Disables the output cache of the current request.
	/// </summary>
	[Named(Constants.NamespaceUri, "disableOutputCache")]
	public class DisableOutputCacheTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			WebUtilities.DisableOutputCache(context);
		}
		#endregion
	}
}