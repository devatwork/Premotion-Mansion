using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Mappers
{
	/// <summary>
	/// Maps <see cref="Negation"/>.
	/// </summary>
	public class NegationMapper : BaseCompositeMapper<Negation>
	{
		#region Constructors
		/// <summary>
		/// Constructs a BaseCompositeMapper.
		/// </summary>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nucleus"/> is null.</exception>
		public NegationMapper(INucleus nucleus) : base(nucleus)
		{
		}
		#endregion
		#region Overrides of BaseSpecificationMapper<Negation>
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="specification"/>.</param>
		protected override void DoMap(IMansionContext context, Query query, Negation specification, SearchQuery searchQuery)
		{
			// map each child
			var filterList = new List<BaseFilter>();
			using (searchQuery.FilterListStack.Push(filterList))
				Mappers.Elect(context, specification.Negated).Map(context, query, specification, searchQuery);

			// add the filters to the composite
			var composite = new NotFilter(filterList.Single());

			// add the filter to the search query
			searchQuery.Add(composite);
		}
		#endregion
	}
}