using System;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Caching
{
	/// <summary>
	/// Clears the entire cache of the current application.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "clearCache")]
	public class ClearCacheTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cachingService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ClearCacheTag(ICachingService cachingService)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values
			this.cachingService = cachingService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// clear all items
			cachingService.ClearAll();
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}