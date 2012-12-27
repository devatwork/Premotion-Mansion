using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Defines the allowed roles <see cref="BaseFilter"/>.
	/// </summary>
	public class AllowedRolesSpecificationMapper : BaseSpecificationMapper<AllowedRolesSpecification>
	{
		#region Overrides of BaseSpecificationMapper<AllowedRolesSpecification>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, AllowedRolesSpecification specification, SearchQuery searchQuery)
		{
			// check if there are no values
			if (specification.RoleIds.Length == 0)
				return;

			// create an OR filter
			var composite = new OrFilter();

			// add all the role IDs as term filters
			composite.Add(specification.RoleIds.Select(roleId => new TermFilter("allowedRoleGuids", roleId)));

			// add the or filter to the search
			searchQuery.Add(composite);
		}
		#endregion
	}
}