using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="LimitQueryComponent"/>s.
	/// </summary>
	public class LimitQueryComponentMapper : QueryComponentMapper<LimitQueryComponent>
	{
		#region Overrides of QueryComponentMapper<LimitQueryComponent>
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="component"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, LimitQueryComponent component, SearchQuery searchQuery)
		{
			searchQuery.Size = component.Limit;
		}
		#endregion
	}
}