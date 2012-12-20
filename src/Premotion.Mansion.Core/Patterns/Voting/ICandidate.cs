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
}