using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns.Voting
{
	/// <summary>
	/// Defines extension methods for several types.
	/// </summary>
	public static class Extensions
	{
		#region Extensions of IEnumerable{ICandidate{TSubject}}
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="candidates">The <see cref="ICandidate{TSubject}"/>s competing in this election.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <returns>Returns the winner of the election.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="candidates"/> or <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if the election did not result in one winner.</exception>
		public static TCandidate Elect<TCandidate, TSubject>(this IEnumerable<TCandidate> candidates, IMansionContext context, TSubject subject) where TCandidate : ICandidate<TSubject>
		{
			// validate arguments
			if (candidates == null)
				throw new ArgumentNullException("candidates");
			if (context == null)
				throw new ArgumentNullException("context");

			var highestVoteResult = VoteResult.Refrain;
			var higestCandidateList = new List<TCandidate>();
			foreach (var candidate in candidates)
			{
				// ask the candidate to vote
				var voteResult = candidate.Vote(context, subject);

				// check for higher vote
				if (voteResult.Strength > highestVoteResult.Strength)
				{
					highestVoteResult = voteResult;
					higestCandidateList.Clear();
					higestCandidateList.Add(candidate);
				}
					// check for equal vote
				else if (voteResult.Strength == highestVoteResult.Strength && voteResult.Strength != VoteResult.Refrain.Strength)
				{
					highestVoteResult = voteResult;
					higestCandidateList.Add(candidate);
				}
			}

			// check if there was a candidate interested at all
			if (highestVoteResult.Strength == VoteResult.Refrain.Strength)
				throw new InvalidOperationException("None of the candidates were interested in the subject.");

			// check for ambigious candidates
			if (higestCandidateList.Count != 1)
				throw new InvalidOperationException("Two ore more candidates were equally interested in the subject.");

			// return the subject
			return higestCandidateList[0];
		}
		#endregion
	}
}