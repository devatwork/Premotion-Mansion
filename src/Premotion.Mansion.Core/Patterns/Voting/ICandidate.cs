namespace Premotion.Mansion.Core.Patterns.Voting
{
	/// <summary>
	/// Represents a voter.
	/// </summary>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public interface ICandidate<in TSubject>
	{
		#region Vote Methods
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		VoteResult Vote(IMansionContext context, TSubject subject);
		#endregion
	}
	/// <summary>
	/// Represents a voter.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IMansionContext"/>.</typeparam>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public interface ICandidate<in TContext, in TSubject> where TContext : class, IMansionContext
	{
		#region Vote Methods
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <typeparamref name="TContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		VoteResult Vote(TContext context, TSubject subject);
		#endregion
	}
}