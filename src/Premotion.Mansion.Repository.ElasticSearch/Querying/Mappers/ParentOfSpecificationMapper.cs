using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="ParentOfSpecification"/>s.
	/// </summary>
	public class ParentOfSpecificationMapper : BaseSpecificationMapper<ParentOfSpecification>
	{
		#region Overrides of BaseSpecificationMapper<ParentOfSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, ParentOfSpecification specification, SearchQuery searchQuery)
		{
			BaseFilter filter;
			if (specification.ChildPointer.HasParent)
			{
				if (specification.Depth.HasValue)
				{
					// calculate the depth
					var depth = specification.Depth.Value < 0 ? Math.Abs(specification.Depth.Value) : specification.ChildPointer.Depth - specification.Depth.Value - 1;

					// create the filter on the pointer
					filter = new TermFilter("pointer", specification.ChildPointer.Pointer[depth]);
				}
				else
				{
					// create a filter on all the parents
					var pointer = new TermsFilter("pointer", specification.ChildPointer.Parent.Pointer.Select(x => (object) x));
					var depth = RangeFilter.LessThan("depth", specification.ChildPointer.Depth);
					filter = new AndFilter().Add(pointer, depth);
				}
			}
			else
			{
				// will never match
				filter = new TermFilter("pointer", -1);
			}

			// wrap the combined filter into a nested filter
			var nested = new NestedFilter("pointer", filter);

			// add the nested filter to the search query
			searchQuery.Add(nested);
		}
		#endregion
	}
}