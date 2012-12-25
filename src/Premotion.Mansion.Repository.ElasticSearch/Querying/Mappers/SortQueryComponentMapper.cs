using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

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
			foreach (var sort in component.Sorts.Select<Sort, BaseSort>(sort =>
			                                                            {
			                                                            	// get the property mapping
			                                                            	var propertyMapping = searchQuery.TypeMapping.FindPropertyMapping<PropertyMapping>(sort.PropertyName);

			                                                            	// create the field sort
			                                                            	return new FieldSort(propertyMapping.SortField, sort);
			                                                            }))
				searchQuery.Add(sort);
		}
		#endregion
	}
}