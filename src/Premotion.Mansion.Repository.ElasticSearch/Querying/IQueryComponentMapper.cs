using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Maps <see cref="QueryComponent"/> to <see cref="SearchQuery"/>.
	/// </summary>
	public interface IQueryComponentMapper : ICandidate<QueryComponent>
	{
		#region Map Methods
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="searchQuery"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="searchQuery">The <see cref="SearchQuery"/> to which to map <paramref name="component"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		void Map(IMansionContext context, Query query, QueryComponent component, SearchQuery searchQuery);
		#endregion
	}
}