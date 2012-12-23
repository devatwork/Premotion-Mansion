using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.ElasticSearch.Querying;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Data
{
	/// <summary>
	/// Implements the <see cref="BaseQueryEngine"/> for ElasticSearch.
	/// </summary>
	public class ElasticSearchQueryEngine : BaseQueryEngine
	{
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="ElasticSearchQueryEngine"/>.
		/// </summary>
		/// <param name="searcher">The <see cref="Searcher"/>.</param>
		/// <param name="indexDefinitionResolver">The <see cref="IndexDefinitionResolver"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		public ElasticSearchQueryEngine(Searcher searcher, IndexDefinitionResolver indexDefinitionResolver) : base(500)
		{
			// validate arguments
			if (searcher == null)
				throw new ArgumentNullException("searcher");
			if (indexDefinitionResolver == null)
				throw new ArgumentNullException("indexDefinitionResolver");

			// set the value
			this.searcher = searcher;
			this.indexDefinitionResolver = indexDefinitionResolver;
		}
		#endregion
		#region Overrides of BaseQueryEngine
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Record DoRetrieveSingle(IMansionContext context, Query query)
		{
			return searcher.Search(context, query).Records.First();
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		protected override RecordSet DoRetrieve(IMansionContext context, Query query)
		{
			return searcher.Search(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			return searcher.SearchNodes(context, query).Nodes.First();
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			return searcher.SearchNodes(context, query);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IMansionContext context, Query subject)
		{
			// find the root type for this query
			var rootType = subject.TypeHints.FindCommonAncestor(context);

			// resolve the index definitions of this record
			var indexDefinitions = indexDefinitionResolver.Resolve(context, rootType);

			// check if there is an index defined for this query
			return indexDefinitions.Any() ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
		#region Private Fields
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		private readonly Searcher searcher;
		#endregion
	}
}