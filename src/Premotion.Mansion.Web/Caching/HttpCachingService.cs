using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using CacheDependency = Premotion.Mansion.Core.Caching.CacheDependency;

namespace Premotion.Mansion.Web.Caching
{
	/// <summary>
	/// Implements <see cref="ICachingService"/> using the HTTP cache.
	/// </summary>
	public class HttpCachingService : ICachingService
	{
		#region Nested Class: CacheContext
		/// <summary>
		/// Defines the context of the cache.
		/// </summary>
		private class CacheContext : ContextExtension
		{
			#region Constructors
			/// <summary>
			/// Constructs the context.
			/// </summary>
			/// <param name="originalContext">The <see cref="IContext"/>.</param>
			public CacheContext(IContext originalContext) : base(originalContext)
			{
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the <see cref="CacheDependencyCollector"/> stack.
			/// </summary>
			public IAutoPopStack<CacheDependencyCollector> Collectors
			{
				get { return collectors; }
			}
			#endregion
			#region private Fields
			private readonly IAutoPopStack<CacheDependencyCollector> collectors = new AutoPopStack<CacheDependencyCollector>();
			#endregion
		}
		#endregion
		#region Nested Class: CacheDependencyCollector
		/// <summary>
		/// Represents a collector of cache dependencies.
		/// </summary>
		private class CacheDependencyCollector
		{
			#region Add Methods
			/// <summary>
			/// Adds a <see cref="Core.Caching.CacheDependency"/> to the collector.
			/// </summary>
			/// <param name="dependency">The <see cref="Core.Caching.CacheDependency"/>.</param>
			public void Add(CacheDependency dependency)
			{
				// validate arguments
				if (dependency == null)
					throw new ArgumentNullException("dependency");
				dependencies.Add(dependency);
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the collected dependencies.
			/// </summary>
			public IEnumerable<CacheDependency> Dependencies
			{
				get { return dependencies; }
			}
			#endregion
			#region Private Fields
			private readonly List<CacheDependency> dependencies = new List<CacheDependency>();
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// Defines the gloval eviction key which is used to evict all items from the cache.
		/// </summary>
		private const string GlobalEvictionKey = "Global_A17DBFAD-B360-4867-868C-783D0EB03434";
		#endregion
		#region Implementation of ICacheService
		/// <summary>
		/// Tries to get an object from this cache by its <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="value">The <see cref="CachedObject{TObject}"/> found in this cache.</param>
		/// <returns>Returns true when the object was found, otherwise false.</returns>
		public bool TryGet<TObject>(IContext context, CacheKey key, out CachedObject<TObject> value)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (key == null)
				throw new ArgumentNullException("key");

			// get the entry
			var cacheKey = key.ToString();
			value = HttpRuntime.Cache.Get(cacheKey) as CachedObject<TObject>;
			return value != null;
		}
		/// <summary>
		/// Gets an object of type <typeparamref name="TObject"/> from the cache byt it's key. If the item does not exist in the cache add and return it using <paramref name="valueFactory"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="key">The cache key.</param>
		/// <param name="valueFactory">The value factory which provides the object if it doesn't exist in this cache.</param>
		/// <returns>Returns the value.</returns>
		public TObject GetOrAdd<TObject>(IContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (key == null)
				throw new ArgumentNullException("key");
			if (valueFactory == null)
				throw new ArgumentNullException("valueFactory");

			// get the entry
			var cacheKey = key.ToString();
			var entry = HttpRuntime.Cache.Get(cacheKey) as CachedObject<TObject>;
			if (entry != null)
				return entry.Object;

			// create the value
			return InsertIntoCache(context, cacheKey, valueFactory);
		}
		/// <summary>
		/// Adds or replaces an object of type <typeparamref name="TObject"/> in the cache.
		/// </summary>
		/// <typeparam name="TObject">The type of stored object.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="valueFactory">The value factory which produces the cache object.</param>
		/// <returns>Returns the value.</returns>
		public TObject AddOrReplace<TObject>(IContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (key == null)
				throw new ArgumentNullException("key");
			if (valueFactory == null)
				throw new ArgumentNullException("valueFactory");

			// first remove the item from cache
			Clear(key);

			// add it back to the cache
			return InsertIntoCache(context, key.ToString(), valueFactory);
		}
		/// <summary>
		/// Clears all the items in the cache matching <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		public void Clear(CacheKey key)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// evict items from the cache
			HttpRuntime.Cache.Remove(key.ToString());
		}
		/// <summary>
		/// Clears all items from the cache.
		/// </summary>
		public void ClearAll()
		{
			Clear((StringCacheKey) GlobalEvictionKey);
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Inserts an object into the cache.
		/// </summary>
		/// <typeparam name="TObject">The type of object which to insert.</typeparam>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="key">The cache key.</param>
		/// <param name="valueFactory">The value factory.</param>
		/// <returns>Returns the cached object.</returns>
		private static TObject InsertIntoCache<TObject>(IContext context, string key, Func<CachedObject<TObject>> valueFactory)
		{
			// create the value
			CachedObject<TObject> entry;
			var cacheEntryCollector = new CacheDependencyCollector();
			var cacheContext = context.Extend(ctx => new CacheContext(context));
			using (cacheContext.Collectors.Push(cacheEntryCollector))
				entry = valueFactory();
			if (entry == null)
				throw new InvalidOperationException("Cached objects can not be null");

			// check if this object is not cachable
			if (!entry.IsCachable)
				return entry.Object;

			// add all collected dependencies
			foreach (var dependency in cacheEntryCollector.Dependencies)
				entry.Add(dependency);

			// get the cache dependency and the sliding expiration
			System.Web.Caching.CacheDependency cacheDependency;
			TimeSpan slidingExpiration;
			ExtractDependencyInfo(entry.Dependencies, out cacheDependency, out slidingExpiration);

			// get the priority
			var priority = GetCacheItemPriority(entry);

			// store the item in the cache
			HttpRuntime.Cache.Insert(key, entry, cacheDependency, Cache.NoAbsoluteExpiration, slidingExpiration, priority, null);

			// add dependencies on upper collectors
			if (entry is IDependableCachedObject)
			{
				foreach (var collector in cacheContext.Collectors)
				{
					foreach (var dependency in entry.Dependencies)
						collector.Add(dependency);
					collector.Add(new StringCacheKeyDependency(key));
				}
			}

			// return the value)
			return entry.Object;
		}
		/// <summary>
		/// Gets the dependency information.
		/// </summary>
		/// <param name="dependencies"></param>
		/// <param name="cacheDependency"></param>
		/// <param name="slidingExpiration"></param>
		private static void ExtractDependencyInfo(IEnumerable<CacheDependency> dependencies, out System.Web.Caching.CacheDependency cacheDependency, out TimeSpan slidingExpiration)
		{
			// validate arguments
			if (dependencies == null)
				throw new ArgumentNullException("dependencies");

			// initialize objects
			slidingExpiration = Cache.NoSlidingExpiration;
			var paths = new List<string>();
			var keys = new List<string> {GlobalEvictionKey};

			// store key in cache when needed
			if (HttpRuntime.Cache[GlobalEvictionKey] == null)
				HttpRuntime.Cache.Insert(GlobalEvictionKey, DateTime.Now, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

			// loop through all the dependencies
			foreach (var dependency in dependencies)
			{
				// check for key dependency
				if (dependency is StringCacheKeyDependency)
				{
					var key = dependency.ToString();

					// store key in cache when needed
					if (HttpRuntime.Cache[key] == null)
						HttpRuntime.Cache.Insert(key, DateTime.Now, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

					keys.Add(key);
				}

				// check for timespan dependency);
				if (dependency is TimeSpanDependency)
					slidingExpiration = ((TimeSpanDependency) dependency).Timespan;
			}

			// create cache dependency
			cacheDependency = new System.Web.Caching.CacheDependency(paths.ToArray(), keys.ToArray());
		}
		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <typeparam name="TObject"></typeparam>
		/// <param name="entry"></param>
		/// <returns></returns>
		private static CacheItemPriority GetCacheItemPriority<TObject>(CachedObject<TObject> entry)
		{
			CacheItemPriority priority;
			switch (entry.Priority)
			{
				case Priority.NotRemovable:
				{
					priority = CacheItemPriority.NotRemovable;
					break;
				}
				case Priority.High:
				{
					priority = CacheItemPriority.High;
					break;
				}
				case Priority.Low:
				{
					priority = CacheItemPriority.Low;
					break;
				}
				case Priority.Normal:
				{
					priority = CacheItemPriority.Normal;
					break;
				}
				default:
					throw new InvalidOperationException("Unknown cache item priority");
			}
			return priority;
		}
		#endregion
	}
}