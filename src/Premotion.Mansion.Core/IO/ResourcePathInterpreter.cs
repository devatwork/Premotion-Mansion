using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Patterns.Interpreting;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Interprets properties into a <see cref="IResourcePath"/>.
	/// </summary>
	[Exported]
	public abstract class ResourcePathInterpreter : IVotingInterpreter<IPropertyBag, IResourcePath>
	{
		#region Implementation of ICandidate<IPropertyBag>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IContext context, IPropertyBag subject)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected abstract VoteResult DoVote(IContext context, IPropertyBag subject);
		#endregion
		#region Implementation of IInterpreter<in IPropertyBag,out IResourcePath>
		/// <summary>
		/// Interprets the input..
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		public IResourcePath Interpret(IContext context, IPropertyBag input)
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
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected abstract IResourcePath DoInterpret(IContext context, IPropertyBag input);
		#endregion
	}
}