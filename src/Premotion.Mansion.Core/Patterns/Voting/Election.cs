using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Core.Patterns.Voting
{
	/// <summary>
	/// Represents an election.
	/// </summary>
	/// <typeparam name="TCandidate">The type of voter.</typeparam>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public static class Election<TCandidate, TSubject> where TCandidate : ICandidate<TSubject>
	{
		#region Solve Methods
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <param name="tieResolver">Invoked when this election is about to end in a tie.</param>
		/// <returns>Returns the winner of the election.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="InconclusiveElectionException{TCandidate,TSubject}">Thrown when there is no candidate interested in the subject.</exception>
		/// <exception cref="TieElectionException{TCandidate,TSubject}">Thrown when two or more candidate were equally intrested in the subject.</exception>
		public static TCandidate Elect(IMansionContext context, IEnumerable<TCandidate> candidates, TSubject subject, Func<IEnumerable<TCandidate>, TCandidate> tieResolver = null)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (candidates == null)
				throw new ArgumentNullException("candidates");

			var highestVoteResult = VoteResult.Refrain;
			var higestCandidateList = new List<TCandidate>();
			var candidateArray = candidates.ToArray();
			foreach (var candidate in candidateArray)
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
				throw new InconclusiveElectionException<TCandidate, TSubject>("None of the candidates were interested in the subject.", candidateArray, subject);

			// check for ambigious candidates
			if (higestCandidateList.Count != 1)
			{
				// check if there is a tie resolver
				if (tieResolver != null)
					return tieResolver(higestCandidateList);
				
				// election ended in a tie
				throw new TieElectionException<TCandidate, TSubject>("Two or more candidates were equally interested in the subject.", higestCandidateList, subject);
			}

			// return the subject
			return higestCandidateList[0];
		}
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <param name="winner">The winning candidate.</param>
		/// <returns>Returns true when a winner was found, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static bool TryElect(IMansionContext context, IEnumerable<TCandidate> candidates, TSubject subject, out TCandidate winner)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (candidates == null)
				throw new ArgumentNullException("candidates");

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
			{
				winner = default(TCandidate);
				return false;
			}

			// check for ambigious candidates
			if (higestCandidateList.Count != 1)
			{
				winner = default(TCandidate);
				return false;
			}

			// execute the action on the candidate
			winner = higestCandidateList[0];
			return true;
		}
		#endregion
	}
}