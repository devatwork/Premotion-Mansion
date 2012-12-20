using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
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
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Map(IMansionContext context, Query query, Specification specification, SearchQuery searchQuery)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			if (specification == null)
				throw new ArgumentNullException("specification");
			if (searchQuery == null)
				throw new ArgumentNullException("searchQuery");

			// Invoke template method
			DoMap(context, query, (TSpecification) specification, searchQuery);
		}
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected abstract void DoMap(IMansionContext context, Query query, TSpecification specification, SearchQuery searchQuery);
		#endregion
	}
}