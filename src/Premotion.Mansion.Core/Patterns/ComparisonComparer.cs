using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns
{
	/// <summary>
	/// Utility to build an IComparer implementation from a Comparison delegate,
	/// and a static method to do the reverse.
	/// </summary>
	public sealed class ComparisonComparer<T> : IComparer<T>
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance which will proxy to the given Comparison
		/// delegate when called.
		/// </summary>
		/// <param name="comparison">Comparison delegate to proxy to. Must not be null.</param>
		public ComparisonComparer(Comparison<T> comparison)
		{
			// validate arguments
			if (comparison == null)
				throw new ArgumentNullException("comparison");

			// set value
			this.comparison = comparison;
		}
		#endregion
		#region Implementation of IComparer{T}
		/// <summary>
		/// Implementation of IComparer.Compare which simply proxies
		/// to the originally specified Comparison delegate.
		/// </summary>
		public int Compare(T x, T y)
		{
			return comparison(x, y);
		}
		#endregion
		#region Private Fields
		private readonly Comparison<T> comparison;
		#endregion
	}
}