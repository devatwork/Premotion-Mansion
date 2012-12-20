using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Converts the given <see cref="Specification"/> to the <see cref="SearchDescriptor"/>.
	/// </summary>
	public interface ISpecificationMapper : ICandidate<Specification>
	{
		#region Map Methods
		/// <summary>
		/// Maps the given <paramref name="specification"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="specification">The source <see cref="Specification"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="specification"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		void Map(IMansionContext context, Query query, Specification specification, SearchDescriptor search);
		#endregion
	}
}