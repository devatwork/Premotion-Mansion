using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps the <see cref="StatusSpecification"/> query.
	/// </summary>
	public class StatusSpecificationMapper : BaseSpecificationMapper<StatusSpecification>
	{
		#region Overrides of BaseSpecificationMapper<StatusSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, StatusSpecification specification, SearchQuery searchQuery)
		{
			// check for any
			if (specification.Status == NodeStatus.Any)
				return;

			// create an OR composite
			var or = new OrFilter();

			// map status: draft
			if ((specification.Status & NodeStatus.Draft) == NodeStatus.Draft)
			{
				// create approved=0 and archived=0 filter
				var approved = new TermFilter("approved", false);
				var archived = new TermFilter("archived", false);
				or.Add(new AndFilter().Add(approved, archived));
			}

			// map status: staged
			if ((specification.Status & NodeStatus.Staged) == NodeStatus.Staged)
			{
				// create approved=1 and publicationDate > now() and archived=0 filter
				var approved = new TermFilter("approved", true);
				var publicationDate = RangeFilter.GreaterThan("publicationDate", DateTime.Now);
				publicationDate.Cache = false;
				var archived = new TermFilter("archived", false);
				or.Add(new AndFilter().Add(approved, publicationDate, archived));
			}

			// map status: published
			if ((specification.Status & NodeStatus.Published) == NodeStatus.Published)
			{
				// create approved=1 and publicationDate <= now() and expirationDate >= now() and archived=0 filter
				var approved = new TermFilter("approved", true);
				var publicationDate = RangeFilter.LessThanOrEqualTo("publicationDate", DateTime.Now);
				var expirationDate = RangeFilter.GreaterThanOrEqualTo("expirationDate", DateTime.Now);
				expirationDate.Cache = false;
				var archived = new TermFilter("archived", false);
				or.Add(new AndFilter().Add(approved, publicationDate, expirationDate, archived));
			}

			// map status: expired
			if ((specification.Status & NodeStatus.Expired) == NodeStatus.Expired)
			{
				// create approved=1 and expirationDate < now() and archived=0 filter
				var approved = new TermFilter("approved", true);
				var expirationDate = RangeFilter.LessThan("expirationDate", DateTime.Now);
				expirationDate.Cache = false;
				var archived = new TermFilter("archived", false);
				or.Add(new AndFilter().Add(approved, expirationDate, archived));
			}

			// map status: archived
			if ((specification.Status & NodeStatus.Archived) == NodeStatus.Archived)
			{
				// create archived=1
				or.Add(new TermFilter("archived", true));
			}

			// append the filter
			searchQuery.Add(or);
		}
		#endregion
	}
}