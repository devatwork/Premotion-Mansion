using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="AutocompleteSpecification"/>.
	/// </summary>
	public class AutocompleteSpecificationMapper : BaseSpecificationMapper<AutocompleteSpecification>
	{
		#region Overrides of BaseSpecificationMapper<AutocompleteSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, AutocompleteSpecification specification, SearchQuery searchQuery)
		{
			// find the field
			var field = specification.PropertyName;

			// find the property mapping
			MultiFieldPropertyMapping multiFieldPropertyMapping;
			if (searchQuery.TypeMapping.TryFindPropertyMapping(specification.PropertyName, out multiFieldPropertyMapping))
			{
				// find the autocomplete field, if available
				var propertyMapping = multiFieldPropertyMapping.Mappings.FirstOrDefault(candidate => candidate.Field.Equals("autocomplete", StringComparison.OrdinalIgnoreCase));
				if (propertyMapping != null)
					field = multiFieldPropertyMapping.Field + "." + propertyMapping.Field;
			}

			// construct the query
			var q = new TextPhrasePrefixQuery(specification.Fragment, field);

			// add to the search query
			searchQuery.Add(q);
		}
		#endregion
	}
}