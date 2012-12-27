using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Specifications;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Defines the allowed roles <see cref="BaseFilter"/>.
	/// </summary>
	public class PhrasePrefixQuerySpecificationMapper : BaseSpecificationMapper<PhrasePrefixQuerySpecification>
	{
		#region Overrides of BaseSpecificationMapper<PhrasePrefixQuerySpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, PhrasePrefixQuerySpecification specification, SearchQuery searchQuery)
		{
			// construct the query
			var q = new TextPhrasePrefixQuery(specification.Query, specification.Field);

			// add to the search query
			searchQuery.Add(q);
		}
		#endregion
	}
}