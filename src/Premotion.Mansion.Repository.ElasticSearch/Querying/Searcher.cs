using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Implements the ElasticSearch searcher interface.
	/// </summary>
	public class Searcher
	{
		#region Constructors
		/// <summary>
		/// Constructs the index service.
		/// </summary>
		/// <param name="connectionManager">The <see cref="connectionManager"/>.</param>
		/// <param name="indexDefinitionResolver">The <see cref="IndexDefinitionResolver"/>.</param>
		/// <param name="converters">The <see cref="IQueryComponentMapper"/>s.</param>
		public Searcher(ConnectionManager connectionManager, IndexDefinitionResolver indexDefinitionResolver, IEnumerable<IQueryComponentMapper> converters)
		{
			// validate arguments
			if (connectionManager == null)
				throw new ArgumentNullException("connectionManager");
			if (indexDefinitionResolver == null)
				throw new ArgumentNullException("indexDefinitionResolver");
			if (converters == null)
				throw new ArgumentNullException("converters");

			// set values
			this.connectionManager = connectionManager;
			this.indexDefinitionResolver = indexDefinitionResolver;
			this.converters = converters.ToArray();
		}
		#endregion
		#region Search Methods
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="context"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public RecordSet Search(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// find the root type for this query
			var rootType = query.TypeHints.FindCommonAncestor(context);

			// resolve the index definitions of this record
			var indexDefinitions = indexDefinitionResolver.Resolve(context, rootType);

			// determine the index which to use to perform the search
			var indexDefinitionTypeMappingPair = SelectBestIndexForQuery(rootType, query, indexDefinitions);

			// create a search descriptor
			var search = new SearchDescriptor(indexDefinitionTypeMappingPair.Item1, indexDefinitionTypeMappingPair.Item2);

			// map all the query components to the search descriptor
			foreach (var component in query.Components)
				converters.Elect(context, component).Map(context, query, component, search);

			// TODO: execute the query
			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Selects the best <see cref="IndexDefinition"/> for the given <paramref name="query"/>.
		/// </summary>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="query">The <see cref="Query"/>.</param>
		/// <param name="indexDefinitions">The candidate <see cref="IndexDefinition"/>s.</param>
		/// <returns>Returns a<see cref="Tuple{IndexDefinition,TypeMapping}"/> containing the <see cref="IndexDefinition"/> and <see cref="TypeMapping"/>.</returns>
		private static Tuple<IndexDefinition, TypeMapping> SelectBestIndexForQuery(ITypeDefinition type, Query query, IEnumerable<IndexDefinition> indexDefinitions)
		{
			// first get the type mapping for the root type
			var indexDefinitionTypeMappingPairs = indexDefinitions.Select(candidate => new Tuple<IndexDefinition, TypeMapping>(candidate, candidate.FindTypeMapping(type.Name)));

			// return the type mapping which has to most fields matching the query
			return indexDefinitionTypeMappingPairs.OrderByDescending(pair => pair.Item2.Properties.Select(prop => prop.Key).Intersect(query.PropertyHints, StringComparer.OrdinalIgnoreCase).Count()).First();
		}
		#endregion
		#region Private Fields
		private readonly ConnectionManager connectionManager;
		private readonly IEnumerable<IQueryComponentMapper> converters;
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		#endregion
	}
}