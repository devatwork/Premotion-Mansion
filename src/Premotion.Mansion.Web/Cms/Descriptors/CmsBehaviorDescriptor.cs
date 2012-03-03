using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Model;

namespace Premotion.Mansion.Web.Cms.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "behavior")]
	public class CmsBehaviorDescriptor : NestedTypeDescriptor
	{
		#region Constants
		/// <summary>
		/// Prefix for cache keys.
		/// </summary>
		private static readonly string cacheKeyPrefix = "ResponseTemplate_" + Guid.NewGuid() + "_";
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public CmsBehaviorDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="CmsBehavior"/> of this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the <see cref="CmsBehavior"/> of this descriptor.</returns>
		public CmsBehavior GetBehavior(MansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// create the cache key
			var cacheKey = cacheKeyPrefix + TypeDefinition.Name;

			// get the behavior from the cache or add it
			var cachingService = context.Nucleus.Get<ICachingService>(context);
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