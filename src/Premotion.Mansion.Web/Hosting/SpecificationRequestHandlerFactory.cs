using System;
using Premotion.Mansion.Core.Patterns.Specifications;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Base class for <see cref="RequestHandlerFactory"/>s which use a <see cref="ISpecification{TSubject,TResult}"/> to determine if th request can be handled by it.
	/// </summary>
	public abstract class SpecificationRequestHandlerFactory : RequestHandlerFactory
	{
		#region Contructors
		/// <summary>
		/// Constructs a <see cref="SpecificationRequestHandlerFactory"/>.
		/// </summary>
		/// <param name="specification">The <see cref="ISpecification{IMansionWebContext, Boolean}"/> agains which to check.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is null.</exception>
		protected SpecificationRequestHandlerFactory(ISpecification<IMansionWebContext, bool> specification)
		{
			// validate arguments
			if (specification == null)
				throw new ArgumentNullException("specification");

			// set values
			this.specification = specification;
		}
		#endregion
		#region Overrides of RequestHandlerFactory
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult Vote(IMansionWebContext context)
		{
			return specification.IsSatisfiedBy(context) ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
		#region Private Fields
		private readonly ISpecification<IMansionWebContext, bool> specification;
		#endregion
	}
}