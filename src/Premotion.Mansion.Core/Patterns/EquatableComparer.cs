using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns
{
	/// <summary>
	/// Utility to build an IEqualityComparer implementation from a Equatable delegate,
	/// and a static method to do the reverse.
	/// </summary>
	public sealed class EquatableComparer<T> : IEqualityComparer<T>
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance which will proxy to the given Comparison
		/// delegate when called.
		/// </summary>
		/// <param name="compare">Comparison delegate to proxy to. Must not be null.</param>
		/// <param name="hashCode">Hash code delegate to proxy to. Must not be null.</param>
		public EquatableComparer(Func<T, T, bool> compare, Func<T, int> hashCode)
		{
			// validate arguments
			if (compare == null)
				throw new ArgumentNullException("compare");
			if (hashCode == null)
				throw new ArgumentNullException("hashCode");

			// set value
			this.compare = compare;
			this.hashCode = hashCode;
		}
		#endregion
		#region Implementation of  IEqualityComparer{T}
		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <returns>
		/// true if the specified objects are equal; otherwise, false.
		/// </returns>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
		public bool Equals(T x, T y)
		{
			return compare(x, y);
		}
		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <returns>
		/// A hash code for the specified object.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
		public int GetHashCode(T obj)
		{
			return hashCode(obj);
		}
		#endregion
		#region Private Fields
		private readonly Func<T, T, bool> compare;
		private readonly Func<T, int> hashCode;
		#endregion
	}
}