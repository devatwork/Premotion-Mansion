using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Factory for <see cref="RequestHandler"/>s.
	/// </summary>
	[Exported(typeof (RequestHandlerFactory))]
	public abstract class RequestHandlerFactory : ICandidate<IMansionWebContext>
	{
		#region Factory Methods
		/// <summary>
		/// Constructs a <see cref="RequestHandler"/>.
		/// </summary>
		/// <param name="applicationContext">The <see cref="IMansionContext"/> of the application.</param>
		/// <returns>Returns the constructed <see cref="RequestHandler"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="applicationContext"/> is null.</exception>
		public RequestHandler Create(IMansionContext applicationContext)
		{
			// validate arguments
			if (applicationContext == null)
				throw new ArgumentNullException("applicationContext");

			// invoke template method
			return DoCreate(applicationContext);
		}
		/// <summary>
		/// Constructs a <see cref="RequestHandler"/>.
		/// </summary>
		/// <param name="applicationContext">The <see cref="IMansionContext"/> of the application.</param>
		/// <returns>Returns the constructed <see cref="RequestHandler"/>.</returns>
		protected abstract RequestHandler DoCreate(IMansionContext applicationContext);
		#endregion
		#region Implementation of ICandidate<in IMansionWebContext>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IMansionContext context, IMansionWebContext subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// invoke template method
			return Vote(subject);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected abstract VoteResult Vote(IMansionWebContext context);
		#endregion
	}
}