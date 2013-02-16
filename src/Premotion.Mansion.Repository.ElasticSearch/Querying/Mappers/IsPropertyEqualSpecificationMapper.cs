using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Defines the allowed roles <see cref="BaseFilter"/>.
	/// </summary>
	public class IsPropertyEqualSpecificationMapper : BaseSpecificationMapper<IsPropertyEqualSpecification>
	{
		#region Overrides of BaseSpecificationMapper<IsPropertyEqualSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, IsPropertyEqualSpecification specification, SearchQuery searchQuery)
		{
			// find the property mapping
			var propertyMapping = searchQuery.TypeMapping.FindPropertyMapping<PropertyMapping>(specification.PropertyName);

			// if the field is analyzed, use a field query, otherwise a term filter
			if (propertyMapping.IsAnalyzed)
			{
				// add a field query
				searchQuery.Add(new QueryFilter(new FieldQuery(propertyMapping.QueryField, propertyMapping.Normalize(context, specification.Value))));
			}
			else
			{
				// add a term filter
				searchQuery.Add(new TermFilter(propertyMapping.QueryField, propertyMapping.Normalize(context, specification.Value)) {
					Cache = false
				});
			}
		}
		#endregion
	}
}