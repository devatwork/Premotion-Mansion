using System;

namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Provides caching services.
	/// </summary>
	public interface ICachingService
	{
		#region Cache Methods
		/// <summary>
		/// Tries to get an object from this cache by its <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="value">The <see cref="CachedObject{TObject}"/> found in this cache.</param>
		/// <returns>Returns true when the object was found, otherwise false.</returns>
		bool TryGet<TObject>(IMansionContext context, CacheKey key, out CachedObject<TObject> value);
		/// <summary>
		/// Gets an object of type <typeparamref name="TObject"/> from the cache byt it's key. If the item does not exist in the cache add and return it using <paramref name="valueFactory"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="valueFactory">The value factory which provides the object if it doesn't exist in this cache.</param>
		/// <returns>Returns the value.</returns>
		TObject GetOrAdd<TObject>(IMansionContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory);
		/// <summary>
		/// Adds or replaces an object of type <typeparamref name="TObject"/> in the cache.
		/// </summary>
		/// <typeparam name="TObject">The type of stored object.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="key">The <see cref="CacheKey"/>.</param>
		/// <param name="valueFactory">The value factory which produces the cache object.</param>
		/// <returns>Returns the value.</returns>
		TObject AddOrReplace<TObject>(IMansionContext context, CacheKey key, Func<CachedObject<TObject>> valueFactory);
		/// <summary>
		/// Clears all the items in the cache matching <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The cache key.</param>
		void Clear(CacheKey key);
		/// <summary>
		/// Clears all items from the cache.
		/// </summary>
		void ClearAll();
		#endregion
	}
}