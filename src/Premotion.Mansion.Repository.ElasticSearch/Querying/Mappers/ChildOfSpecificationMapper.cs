using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="ChildOfSpecification"/>.
	/// </summary>
	public class ChildOfSpecificationMapper : BaseSpecificationMapper<ChildOfSpecification>
	{
		#region Overrides of BaseSpecificationMapper<ChildOfSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, ChildOfSpecification specification, SearchQuery searchQuery)
		{
			// create the parent id term filter
			var parentId = new TermFilter("pointer", specification.ParentPointer.Id);

			// if there is a specified depth combine it with the parentId
			BaseFilter combined;
			if (specification.Depth.HasValue)
			{
				// create the depth
				var depth = new TermFilter("depth", specification.ParentPointer.Depth + specification.Depth.Value);

				// combine the parentId and depth
				combined = new AndFilter().Add(parentId, depth);
			}
			else
			{
				// create the depth
				var depth = RangeFilter.GreaterThan("depth", specification.ParentPointer.Depth);

				// combine the parentId and depth
				combined = new AndFilter().Add(parentId, depth);
			}

			// wrap the combined filter into a nested filter
			var nested = new NestedFilter("pointer", combined);

			// add the nested filter to the search query
			searchQuery.Add(nested);
		}
		#endregion
	}
}