using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Defines a comparer.
	/// </summary>
	public interface IComparer : ICandidate<Type>
	{
		#region Comparer Methods
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		int Compare(IMansionContext context, object left, object right);
		#endregion
	}
}