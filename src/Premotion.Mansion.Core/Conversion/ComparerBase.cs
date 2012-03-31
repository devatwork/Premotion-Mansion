using System;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Base class for all <see cref="IComparer"/>s.
	/// </summary>
	/// <typeparam name="TType">The type being compared by this comparer.</typeparam>
	[Exported(typeof (IComparer))]
	public abstract class ComparerBase<TType> : IComparer
	{
		#region Implementation of ICandidate<in Type>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IMansionContext context, Type subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");
			return DoVote(context, subject);
		}
		#endregion
		#region Implementation of IComparer
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		public int Compare(IMansionContext context, object left, object right)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (left == null)
				throw new ArgumentNullException("left");
			if (right == null)
				throw new ArgumentNullException("right");
			return Compare(context, (TType) left, (TType) right);
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected virtual VoteResult DoVote(IMansionContext context, Type subject)
		{
			return typeof (TType) == subject ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="left">The left-hand object which to compare.</param>
		/// <param name="right">The right-hand object which to compare.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		protected abstract int Compare(IMansionContext context, TType left, TType right);
		#endregion
	}
}