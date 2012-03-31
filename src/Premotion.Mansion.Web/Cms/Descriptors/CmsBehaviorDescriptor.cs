using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Model;

namespace Premotion.Mansion.Web.Cms.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "behavior")]
	public class CmsBehaviorDescriptor : NestedTypeDescriptor
	{
		#region Constants
		/// <summary>
		/// Prefix for cache keys.
		/// </summary>
		private static readonly string CacheKeyPrefix = "ResponseTemplate_" + Guid.NewGuid() + "_";
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="CmsBehavior"/> of this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="CmsBehavior"/> of this descriptor.</returns>
		public CmsBehavior GetBehavior(IMansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// create the cache key
			var cacheKey = CacheKeyPrefix + TypeDefinition.Name;

			// get the behavior from the cache or add it
			var cachingService = context.Nucleus.ResolveSingle<ICachingService>();
			return cachingService.GetOrAdd(context, (StringCacheKey) cacheKey, () =>
			                                                                   {
			                                                                   	// get the behavior
			                                                                   	var behavior = CmsBehavior.Create(context, this);

			                                                                   	// return cached version
			                                                                   	return new CachedObject<CmsBehavior>(behavior, Priority.NotRemovable);
			                                                                   });
		}
		#endregion
	}
}