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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		VoteResult Vote(IContext context, TSubject subject);
		#endregion
	}
	/// <summary>
	/// Represents a voter.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IContext"/>.</typeparam>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public interface ICandidate<in TContext, in TSubject> where TContext : class, IContext
	{
		#region Vote Methods
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		VoteResult Vote(TContext context, TSubject subject);
		#endregion
	}
}