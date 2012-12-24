using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="FullTextSearchSpecification"/>.
	/// </summary>
	public class FullTextSearchSpecificationMapper : BaseSpecificationMapper<FullTextSearchSpecification>
	{
		#region Overrides of BaseSpecificationMapper<FullTextSearchSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, FullTextSearchSpecification specification, SearchQuery searchQuery)
		{
			// create the query
			var match = new MultiMatchQuery(specification.Query, specification.GetPropertyHints().Select(property => searchQuery.TypeMapping.FindPropertyMapping<PropertyMapping>(property).QueryField));

			// add to the search query
			searchQuery.Add(match);
		}
		#endregion
	}
}