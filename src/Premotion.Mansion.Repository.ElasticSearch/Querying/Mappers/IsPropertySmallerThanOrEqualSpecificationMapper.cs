using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Defines the allowed roles <see cref="IsPropertySmallerThanOrEqualSpecification"/>.
	/// </summary>
	public class IsPropertySmallerThanOrEqualSpecificationMapper : BaseSpecificationMapper<IsPropertySmallerThanOrEqualSpecification>
	{
		#region Overrides of BaseSpecificationMapper<IsPropertySmallerThanOrEqualSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, IsPropertySmallerThanOrEqualSpecification specification, SearchQuery searchQuery)
		{
			// find the property mapping
			var propertyMapping = searchQuery.TypeMapping.FindPropertyMapping<PropertyMapping>(specification.PropertyName);

			// add a range filter
			searchQuery.Add(RangeFilter.LessThanOrEqualTo(propertyMapping.QueryField, propertyMapping.Normalize(context, specification.Value)));
		}
		#endregion
	}
}