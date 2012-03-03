using System;

namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Implements <see cref="CacheDependency"/> for <see cref="StringCacheKey" />.
	/// </summary>
	public class StringCacheKeyDependency : CacheDependency
	{
		#region Constructors
		/// <summary>
		/// Constructs a string cache key dependency.
		/// </summary>
		/// <param name="key">The key.</param>
		public StringCacheKeyDependency(StringCacheKey key)
		{
			// validate arguments
			if (key == null)
				throw new ArgumentNullException("key");

			// set values
			this.key = key;
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return key.ToString();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="CacheKey"/> of the this dependency.
		/// </summary>
		public CacheKey Key
		{
			get { return key; }
		}
		#endregion
		#region Private Fields
		private readonly StringCacheKey key;
		#endregion
	}
}