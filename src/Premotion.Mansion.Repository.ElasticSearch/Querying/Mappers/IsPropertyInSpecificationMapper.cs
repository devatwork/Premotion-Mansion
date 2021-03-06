﻿using System.Linq;
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
	public class IsPropertyInSpecificationMapper : BaseSpecificationMapper<IsPropertyInSpecification>
	{
		#region Overrides of BaseSpecificationMapper<IsPropertyInSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, IsPropertyInSpecification specification, SearchQuery searchQuery)
		{
			// find the property mapping
			var propertyMapping = searchQuery.TypeMapping.FindPropertyMapping<PropertyMapping>(specification.PropertyName);

			// get the normalized properties
			var normalized = specification.Values.Select(value => propertyMapping.Normalize(context, value)).ToList();
			if (normalized.Count == 0)
			{
				// do not match at all
				searchQuery.Add(new NotFilter(new MatchAllFilter()));
			}

			// if the field is analyzed, use a field query, otherwise a term filter
			if (propertyMapping.IsAnalyzed)
			{
				// construct the query
				var q = string.Join(" ", normalized.Select(value => "+" + value));

				// add a field query
				searchQuery.Add(new QueryFilter(new FieldQuery(propertyMapping.QueryField, q)));
			}
			else
			{
				// add a term filter
				searchQuery.Add(new TermsFilter(propertyMapping.QueryField, normalized) {
					Cache = false
				});
			}
		}
		#endregion
	}
}