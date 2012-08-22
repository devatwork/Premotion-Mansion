using System;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Core.Data.Caching
{
	/// <summary>
	/// Provides extension methods used by <see cref="CachingRepositoryDecorator"/>.
	/// </summary>
	public static class CacheExtensions
	{
		#region Extensions for Query
		/// <summary>
		/// Calculates a node cache key for the given <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="Query"/> for which to generate the <see cref="CacheKey"/>.</param>
		/// <param name="prefix">The prefix of the cache query.</param>
		/// <returns>Returns the calculated <see cref="CacheKey"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> or <paramref name="prefix"/> is null.</exception>
		public static CacheKey CalculateCacheKey(this Query query, string prefix)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");
			if (string.IsNullOrEmpty(prefix))
				throw new ArgumentNullException("prefix");

			// generate the cache key
			return (StringCacheKey) (prefix + query);
		}
		/// <summary>
		/// Checks whether the given <paramref name="query"/> is cacheable.
		/// </summary>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <returns>Returns true when the query is cacheable, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
		public static bool Iscacheable(this Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// check if all of the CacheQueryComponents are enabled
			return query.Components.OfType<CacheQueryComponent>().All(candidate => candidate.IsEnabled);
		}
		#endregion
		#region Extensions for Record
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Record"/> for which to create the cacheable object.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<Record> AsCacheableObject(this Record obj)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");

			// create a new cacheable object
			var cacheable = new CachedObject<Record>(obj);

			// add the repository modified cache key
			cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);

			// return the cacheable  object
			return cacheable;
		}
		/// <summary>
		/// Clears the given <paramref name="record"/> from the <paramref name="cachingService"/>.
		/// </summary>
		/// <param name="record">The <see cref="Record"/> which should be cleared from the cache.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void ClearFromCache(this Record record, ICachingService cachingService)
		{
			// validate arguments
			if (record == null)
				throw new ArgumentNullException("record");
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// fire the repository modified
			cachingService.Clear(CachingRepositoryDecorator.RepositoryModifiedDependency.Key);
		}
		#endregion
		#region Extensions for Nodeset
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Nodeset"/> for which to create the cacheable object.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<RecordSet> AsCacheableObject(this RecordSet obj)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");

			// create a new cacheable object
			var cacheable = new CachedObject<RecordSet>(obj);

			// add the repository modified cache key
			cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);

			// return the cacheable  object
			return cacheable;
		}
		#endregion
		#region Extensions for Node
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Node"/> for which to create the cacheable object.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<Node> AsCacheableObject(this Node obj)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");

			// create a new cacheable object
			var cacheable = new CachedObject<Node>(obj);

			// add the repository modified cache key
			cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);

			// return the cacheable  object
			return cacheable;
		}
		/// <summary>
		/// Clears the given <paramref name="node"/> from the <paramref name="cachingService"/>.
		/// </summary>
		/// <param name="node">The <see cref="Node"/> which should be cleared from the cache.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void ClearFromCache(this Node node, ICachingService cachingService)
		{
			// validate arguments
			if (node == null)
				throw new ArgumentNullException("node");
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// fire the repository modified
			cachingService.Clear(CachingRepositoryDecorator.RepositoryModifiedDependency.Key);
		}
		#endregion
		#region Extensions for Nodeset
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Nodeset"/> for which to create the cacheable object.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<Nodeset> AsCacheableObject(this Nodeset obj)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");

			// create a new cacheable object
			var cacheable = new CachedObject<Nodeset>(obj);

			// add the repository modified cache key
			cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);

			// return the cacheable  object
			return cacheable;
		}
		#endregion
	}
}