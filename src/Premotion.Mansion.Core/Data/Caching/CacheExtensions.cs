using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

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
		public static bool IsCacheable(this Query query)
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
		/// <param name="query">The <see cref="Query"/> which resulted in the given <paramref name="obj"/>.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		public static CachedObject<Record> AsCacheableObject(this Record obj, Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// create a new cacheable object
			var cacheable = new CachedObject<Record>(obj);

			// if the result is found, cache it by it's id
			ChildOfSpecification childOfSpecification;
			if (obj != null)
			{
				// generate an ID for this specific record
				var recordIdCacheKey = obj.CalculateIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) recordIdCacheKey);
			}
			else if (query.TryGetSpecification(out childOfSpecification))
			{
				// cache on the parent tree Id
				var parentTreeIdCacheKey = childOfSpecification.ParentPointer.CalculateTreeIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) parentTreeIdCacheKey);
			}
			else
			{
				// add the repository modified cache key
				cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);
			}

			// return the cacheable  object
			return cacheable;
		}
		/// <summary>
		/// Calculates an <see cref="CacheKey"/> on the <paramref name="record"/>'s ID.
		/// </summary>
		/// <param name="record">The <see cref="Record"/> for which to calculate a cache key.</param>
		/// <returns>Returns the calculated <see cref="CacheKey"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="record"/> is null.</exception>
		public static StringCacheKey CalculateIdCacheKey(this Record record)
		{
			// validate arguments
			if (record == null)
				throw new ArgumentNullException("record");

			// return the generated cache key
			return "Record_ID_" + record.Id;
		}
		/// <summary>
		/// Clears the given <paramref name="record"/> from the <paramref name="cachingService"/>.
		/// </summary>
		/// <param name="record">The <see cref="Record"/> which should be cleared from the cache.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void ClearFromCache(this Record record, IMansionContext context, ICachingService cachingService)
		{
			// validate arguments
			if (record == null)
				throw new ArgumentNullException("record");
			if (context == null)
				throw new ArgumentNullException("context");
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// fire the evict by ID
			cachingService.Clear(record.CalculateIdCacheKey());

			// fire the evict by tree ID, if any
			NodePointer pointer;
			if (record.TryGet(context, "pointer", out pointer))
			{
				foreach (var treeCacheKey in pointer.CalculateTreeIdCacheKeys())
					cachingService.Clear(treeCacheKey);
			}

			// fire the repository modified
			cachingService.Clear(CachingRepositoryDecorator.RepositoryModifiedDependency.Key);
		}
		#endregion
		#region Extensions for RecordSet
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="RecordSet"/> for which to create the cacheable object.</param>
		/// <param name="query">The <see cref="Query"/> which resulted in the given <paramref name="obj"/>.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<RecordSet> AsCacheableObject(this RecordSet obj, Query query)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (query == null)
				throw new ArgumentNullException("query");

			// create a new cacheable object
			var cacheable = new CachedObject<RecordSet>(obj);

			ChildOfSpecification childOfSpecification;
			if (query.TryGetSpecification(out childOfSpecification))
			{
				// cache on the parent tree Id
				var parentTreeIdCacheKey = childOfSpecification.ParentPointer.CalculateTreeIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) parentTreeIdCacheKey);
			}
			else
			{
				// add the repository modified cache key
				cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);
			}

			// return the cacheable  object
			return cacheable;
		}
		#endregion
		#region Extensions for Node
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Node"/> for which to create the cacheable object.</param>
		/// <param name="query">The <see cref="Query"/> which resulted in the given <paramref name="obj"/>.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		public static CachedObject<Node> AsCacheableObject(this Node obj, Query query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// create a new cacheable object
			var cacheable = new CachedObject<Node>(obj);

			// if the result is found, cache it by it's id
			ChildOfSpecification childOfSpecification;
			if (obj != null)
			{
				// generate an ID for this specific record
				var recordIdCacheKey = obj.CalculateIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) recordIdCacheKey);
			}
			else if (query.TryGetSpecification(out childOfSpecification))
			{
				// cache on the parent tree Id
				var parentTreeIdCacheKey = childOfSpecification.ParentPointer.CalculateTreeIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) parentTreeIdCacheKey);
			}
			else
			{
				// add the repository modified cache key
				cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);
			}

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

			// fire the evict by ID
			cachingService.Clear(node.CalculateIdCacheKey());

			// fire the evict by tree ID
			foreach (var treeCacheKey in node.Pointer.CalculateTreeIdCacheKeys())
				cachingService.Clear(treeCacheKey);

			// fire the repository modified
			cachingService.Clear(CachingRepositoryDecorator.RepositoryModifiedDependency.Key);
		}
		#endregion
		#region Extensions for Nodeset
		/// <summary>
		/// Turns the given <paramref name="obj"/> in a cacheable object.
		/// </summary>
		/// <param name="obj">The <see cref="Nodeset"/> for which to create the cacheable object.</param>
		/// <param name="query">The <see cref="Query"/> which resulted in the given <paramref name="obj"/>.</param>
		/// <returns>Returns the <see cref="CachedObject{TObject}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
		public static CachedObject<Nodeset> AsCacheableObject(this Nodeset obj, Query query)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");
			if (query == null)
				throw new ArgumentNullException("query");

			// create a new cacheable object
			var cacheable = new CachedObject<Nodeset>(obj);

			ChildOfSpecification childOfSpecification;
			if (query.TryGetSpecification(out childOfSpecification))
			{
				// cache on the parent tree Id
				var parentTreeIdCacheKey = childOfSpecification.ParentPointer.CalculateTreeIdCacheKey();

				// add that cache key as the dependency
				cacheable.Add((StringCacheKeyDependency) parentTreeIdCacheKey);
			}
			else
			{
				// add the repository modified cache key
				cacheable.Add(CachingRepositoryDecorator.RepositoryModifiedDependency);
			}

			// return the cacheable  object
			return cacheable;
		}
		#endregion
		#region Extension for NodePointer
		/// <summary>
		/// Calculates the tree ID cache key for the given <paramref name="pointer"/>.
		/// </summary>
		/// <param name="pointer">The <see cref="NodePointer"/> for which to calculate the tree ID cache key.</param>
		/// <returns>Returns the calculated cache key.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="pointer"/> is null.</exception>
		private static StringCacheKey CalculateTreeIdCacheKey(this NodePointer pointer)
		{
			// validate arguments
			if (pointer == null)
				throw new ArgumentNullException("pointer");

			// calculate the key
			return "Tree_ID_" + pointer.Id;
		}
		/// <summary>
		/// Calculates the tree ID cache keys for the given <paramref name="pointer"/>.
		/// </summary>
		/// <param name="pointer">The <see cref="NodePointer"/> for which to calculate the tree ID cache keys.</param>
		/// <returns>Returns the calculated cache keys.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="pointer"/> is null.</exception>
		private static IEnumerable<StringCacheKey> CalculateTreeIdCacheKeys(this NodePointer pointer)
		{
			// validate arguments
			if (pointer == null)
				throw new ArgumentNullException("pointer");

			// calculate a key for each parent
			return pointer.HierarchyReverse.Select(p => p.CalculateTreeIdCacheKey());
		}
		#endregion
	}
}