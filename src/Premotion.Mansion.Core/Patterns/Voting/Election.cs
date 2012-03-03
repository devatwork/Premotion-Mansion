using System;
using System.Collections.Generic;

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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <returns>Returns the winner of the election.</returns>
		public static TCandidate Elect(IContext context, IEnumerable<TCandidate> candidates, TSubject subject)
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
				throw new InvalidOperationException("None of the candidates were interested in the subject.");

			// check for ambigious candidates
			if (higestCandidateList.Count != 1)
				throw new InvalidOperationException("Two ore more candidates were equally interested in the subject.");

			// return the subject
			return higestCandidateList[0];
		}
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <param name="winner">The winning candidate.</param>
		/// <returns>Returns true when a winner was found, otherwise false.</returns>
		public static bool TryElect(IContext context, IEnumerable<TCandidate> candidates, TSubject subject, out TCandidate winner)
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
	/// <summary>
	/// Represents an election.
	/// </summary>
	/// <typeparam name="TContext">The type of <see cref="IContext"/>.</typeparam>
	/// <typeparam name="TCandidate">The type of voter.</typeparam>
	/// <typeparam name="TSubject">The type of subject on which to vote.</typeparam>
	public static class Election<TContext, TCandidate, TSubject> where TCandidate : ICandidate<TContext, TSubject> where TContext : class, IContext
	{
		#region Solve Methods
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <returns>Returns the winner of the election.</returns>
		public static TCandidate Elect(TContext context, IEnumerable<TCandidate> candidates, TSubject subject)
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
				throw new InvalidOperationException("None of the candidates were interested in the subject.");

			// check for ambigious candidates
			if (higestCandidateList.Count != 1)
				throw new InvalidOperationException("Two ore more candidates were equally interested in the subject.");

			// return the subject
			return higestCandidateList[0];
		}
		/// <summary>
		/// Solves the election.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <param name="winner">The winning candidate.</param>
		/// <returns>Returns true when a winner was found, otherwise false.</returns>
		public static bool TryElect(TContext context, IEnumerable<TCandidate> candidates, TSubject subject, out TCandidate winner)
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