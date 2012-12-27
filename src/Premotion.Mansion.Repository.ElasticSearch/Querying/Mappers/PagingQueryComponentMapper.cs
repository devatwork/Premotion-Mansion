using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="PagingQueryComponent"/>s.
	/// </summary>
	public class PagingQueryComponentMapper : QueryComponentMapper<PagingQueryComponent>
	{
		#region Overrides of QueryComponentMapper<PagingQueryComponent>
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="component"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, PagingQueryComponent component, SearchQuery searchQuery)
		{
			searchQuery.From = (component.PageNumber*component.PageSize) - component.PageSize;
			searchQuery.Size = component.PageSize;
		}
		#endregion
	}
}