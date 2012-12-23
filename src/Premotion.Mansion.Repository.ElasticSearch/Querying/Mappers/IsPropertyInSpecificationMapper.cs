﻿using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

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
			var propertyMapping = searchQuery.TypeMapping.FindPropertyMapping<SinglePropertyMapping>(specification.PropertyName);

			// add a term filter
			searchQuery.Add(new TermsFilter(specification.PropertyName, specification.Values.Select(propertyMapping.Normalize))
			                {
			                	Cache = false
			                });
		}
		#endregion
	}
}