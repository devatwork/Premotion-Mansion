using System;

namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Implements <see cref="CacheKey"/> using strings.
	/// </summary>
	public class StringCacheKey : CacheKey
	{
		#region Constructors
		/// <summary>
		/// Constructs a string cache key.
		/// </summary>
		/// <param name="key">The key.</param>
		private StringCacheKey(string key)
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
		#region Operators
		///<summary>
		/// Converts <paramref name="key"/> into a <see cref="StringCacheKey"/>.
		///</summary>
		///<param name="key">The key.</param>
		///<returns>Returns the converted cache key.</returns>
		public static implicit operator StringCacheKey(string key)
		{
			return new StringCacheKey(key);
		}
		#endregion
		#region Private Fields
		private readonly string key;
		#endregion
	}
}