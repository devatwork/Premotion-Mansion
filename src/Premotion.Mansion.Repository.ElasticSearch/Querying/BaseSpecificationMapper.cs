using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Base class for <see cref="ISpecificationMapper"/>.
	/// </summary>
	[Exported(typeof (ISpecificationMapper))]
	public abstract class BaseSpecificationMapper<TSpecification> : ISpecificationMapper where TSpecification : Specification
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
		/// Maps the given <paramref name="specification"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Map(IMansionContext context, Query query, Specification specification, SearchDescriptor search)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			if (specification == null)
				throw new ArgumentNullException("specification");
			if (search == null)
				throw new ArgumentNullException("search");

			// Invoke template method
			DoMap(context, query, (TSpecification) specification, search);
		}
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="specification"/>.</param>
		protected abstract void DoMap(IMansionContext context, Query query, TSpecification specification, SearchDescriptor search);
		#endregion
	}
}