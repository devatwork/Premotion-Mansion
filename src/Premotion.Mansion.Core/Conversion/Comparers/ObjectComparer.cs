using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion.Comparers
{
	/// <summary>
	/// Implements <see cref="ComparerBase{TType}"/> for <see cref="IComparable"/>.
	/// </summary>
	public class ObjectComparer : ComparerBase<IComparable>
	{
		#region Overrides of ComparerBase<IComparable>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IContext context, Type subject)
		{
			return typeof (IComparable).IsAssignableFrom(subject) ? VoteResult.LowInterest : VoteResult.Refrain;
		}
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		protected override int Compare(IContext context, IComparable left, IComparable right)
		{
			return left.CompareTo(right);
		}
		#endregion
	}
}