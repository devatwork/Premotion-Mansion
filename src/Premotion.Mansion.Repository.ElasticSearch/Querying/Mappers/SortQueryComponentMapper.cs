using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="SortQueryComponent"/>s.
	/// </summary>
	public class SortQueryComponentMapper : QueryComponentMapper<SortQueryComponent>
	{
		#region Overrides of QueryComponentMapper<SortQueryComponent>
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="component"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, SortQueryComponent component, SearchQuery searchQuery)
		{
			foreach (var sort in component.Sorts.Select(sort => new FieldSort(sort)))
				searchQuery.Add(sort);
		}
		#endregion
	}
}