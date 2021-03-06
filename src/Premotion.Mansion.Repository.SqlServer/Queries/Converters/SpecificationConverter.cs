using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements the base class for all <see cref="ISpecificationConverter"/>s.
	/// </summary>
	/// <typeparam name="TSpecification">The type of <see cref="Specification"/> converted by this specification.</typeparam>
	[Exported(typeof (ISpecificationConverter))]
	public abstract class SpecificationConverter<TSpecification> : ISpecificationConverter where TSpecification : Specification
	{
		#region Implementation of ICandidate<in Specification>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IMansionContext context, Specification subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// check if the subject can be mapped by this converter.
			return subject is TSpecification ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Convert(IMansionContext context, Specification specification, QueryCommandContext commandContext)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (specification == null)
				throw new ArgumentNullException("specification");
			if (commandContext == null)
				throw new ArgumentNullException("commandContext");

			// invoke template method
			DoConvert(context, (TSpecification) specification, commandContext);
		}
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected abstract void DoConvert(IMansionContext context, TSpecification specification, QueryCommandContext commandContext);
		#endregion
	}
}