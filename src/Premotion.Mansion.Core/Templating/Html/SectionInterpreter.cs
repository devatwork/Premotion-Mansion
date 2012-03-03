using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Patterns.Interpreting;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Templating.Html
{
	/// <summary>
	/// Represents an interpreter for sections.
	/// </summary>
	[Exported]
	public abstract class SectionInterpreter : IVotingInterpreter<MansionContext, string, Section>
	{
		#region Implementation of ICandidate<string>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(MansionContext context, string subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// invoke template method
			return DoVote(context, subject);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected abstract VoteResult DoVote(MansionContext context, string subject);
		#endregion
		#region Implementation of IInterpreter<in string,out Section>
		/// <summary>
		/// Interprets the input.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		public Section Interpret(MansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			// invoke template method
			return DoInterpret(context, input);
		}
		/// <summary>
		/// Interprets the input..
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected abstract Section DoInterpret(MansionContext context, string input);
		#endregion
	}
}