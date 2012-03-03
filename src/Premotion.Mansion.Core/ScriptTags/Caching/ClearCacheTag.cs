using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Caching
{
	/// <summary>
	/// Clears the entire cache of the current application.
	/// </summary>
	[Named(Constants.NamespaceUri, "clearCache")]
	public class ClearCacheTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the caching service
			var cachingService = context.Nucleus.Get<ICachingService>(context);

			// clear all items
			cachingService.ClearAll();
		}
		#endregion
	}
}