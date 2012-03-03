using System;
using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Implements <see cref="CacheKey"/> for <see cref="IResource"/>.
	/// </summary>
	public class ResourceCacheKey : CacheKey
	{
		#region Constructors
		/// <summary>
		/// Constructs a string cache key.
		/// </summary>
		/// <param name="key">The key.</param>
		private ResourceCacheKey(string key)
		{
			// validate arguments
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			// set values
			this.key = key;
		}
		#endregion
		#region ToString Methods
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return key;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Generates a key for the <paramref name="resource"/>.
		/// </summary>
		/// <param name="resource">The resource.</param>
		/// <returns>Returns the generated key.</returns>
		public static ResourceCacheKey Create(IResource resource)
		{
			// validate arguments
			if (resource == null)
				throw new ArgumentNullException("resource");

			return new ResourceCacheKey("Resource_" + resource.GetResourceIdentifier());
		}
		#endregion
		#region Private Fields
		private readonly string key;
		#endregion
	}
}