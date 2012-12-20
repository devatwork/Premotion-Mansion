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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="candidates">The candidates in this election.</param>
		/// <param name="subject">The subject on which is voted.</param>
		/// <returns>Returns the winner of the election.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="InconclusiveElectionException{TCandidate,TSubject}">Thrown when there is no candidate interested in the subject.</exception>
		/// <exception cref="TieElectionException{TCandidate,TSubject}">Thrown when two or more candidate were equally intrested in the subject.</exception>
		public static TCandidate Elect<TCandidate, TSubject>(this IEnumerable<TCandidate> candidates, IMansionContext context, TSubject subject) where TCandidate : ICandidate<TSubject>
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (candidates == null)
				throw new ArgumentNullException("candidates");

			return Election<TCandidate, TSubject>.Elect(context, candidates, subject);
		}
		#endregion
	}
}