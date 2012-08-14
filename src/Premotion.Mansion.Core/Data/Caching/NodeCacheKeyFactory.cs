using System;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Creates <see cref="CacheKey"/>s for <see cref="Node"/>s and <see cref="Nodeset"/>s.
	/// </summary>
	public static class NodeCacheKeyFactory
	{
		#region Constants
		/// <summary>
		/// Defines the cache key prefix.
		/// </summary>
		private const string RepositoryCacheKeyPrefix = "Repository_";
		/// <summary>
		/// Defines the node key prefix.
		/// </summary>
		private const string NodeCacheKeyPrefix = RepositoryCacheKeyPrefix + "Node_";
		/// <summary>
		/// Defines the nodeset key prefix.
		/// </summary>
		private const string NodesetCacheKeyPrefix = RepositoryCacheKeyPrefix + "Nodeset_";
		/// <summary>
		/// Defines the repository modified cache key.
		/// </summary>
		public static readonly StringCacheKeyDependency RepositoryModifiedDependency = new StringCacheKeyDependency(RepositoryCacheKeyPrefix + "Modified");
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates a <see cref="CacheKey"/> for a <see cref="Node"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/> for which to generate a cache key.</param>
		/// <returns>Returns the generated cache key.</returns>
		public static CacheKey CreateForNode(Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			return (StringCacheKey) (NodeCacheKeyPrefix + query);
		}
		/// <summary>
		/// Creates a <see cref="CacheKey"/> for a <see cref="Nodeset"/> for the specified <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/> for which to generate a cache key.</param>
		/// <returns>Returns the generated cache key.</returns>
		public static CacheKey CreateForNodeset(Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			return (StringCacheKey) (NodesetCacheKeyPrefix + query);
		}
		#endregion
	}
}