using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="FacetQueryComponent"/>s.
	/// </summary>
	public class FacetQueryComponentMapper : QueryComponentMapper<FacetQueryComponent>
	{
		#region Overrides of QueryComponentMapper<FacetQueryComponent>
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="component"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, FacetQueryComponent component, SearchQuery searchQuery)
		{
			// create the terms facet
			var facet = new TermsFacet(component.Facet);

			// add the facet to the query
			searchQuery.Add(facet);
		}
		#endregion
	}
}