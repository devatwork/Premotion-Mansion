using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Responses;
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
		/// Performs a search using the specified <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> on which to search.</param>
		/// <returns>Returns the resulting <see cref="RecordSet"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="ConnectionException">Thrown if a error occurred while executing the search query.</exception>
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
			var search = new SearchQuery(indexDefinitionTypeMappingPair.Item1, indexDefinitionTypeMappingPair.Item2);

			// map all the query components to the search descriptor
			foreach (var component in query.Components)
				converters.Elect(context, component).Map(context, query, component, search);

			// execute the query
			return Search(context, search);
		}
		/// <summary>
		/// Performs a search using the specified <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> on which to search.</param>
		/// <returns>Returns the resulting <see cref="RecordSet"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="ConnectionException">Thrown if a error occurred while executing the search query.</exception>
		public RecordSet Search(IMansionContext context, SearchQuery query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// build the resource
			var resource = query.IndexDefinition.Name + "/_search";

			// execute the search
			var response = connectionManager.Post<SearchResponse>(resource, query);

			// create the record set
			return MapRecordSet(context, query, response);
		}
		#endregion
		#region Index Definition Methods
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
		#region Map Methods
		/// <summary>
		/// Maps the given <paramref name="response"/> into a <see cref="RecordSet"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="response">The <see cref="SearchResponse"/>.</param>
		/// <returns>Returns the mapped record set.</returns>
		private static RecordSet MapRecordSet(IMansionContext context, SearchQuery query, SearchResponse response)
		{
			// map all the hits
			var records = MapRecords(context, query, response.Hits.Hits);

			// map the set metadata
			var metaData = MapRecordSetMetaData(query, response.Hits);

			// create and return the set
			return new RecordSet(context, metaData, records);
		}
		/// <summary>
		/// Maps the meta data of the <paramref name="hits"/>.
		/// </summary>
		/// <param name="query">The source <see cref="SearchQuery"/>.</param>
		/// <param name="hits">The resulting <see cref="HitMetaData"/>.</param>
		/// <returns>Returns a <see cref="IPropertyBag"/> containing the meta data.</returns>
		private static IPropertyBag MapRecordSetMetaData(SearchQuery query, HitMetaData hits)
		{
			// create the meta data
			var metaData = new PropertyBag
			               {
			               	{"totalCount", hits.Total}
			               };

			// set the paging options if any
			if (query.From.HasValue && query.Size.HasValue)
			{
				metaData.Set("pageNumber", (query.From.Value + query.Size.Value)/query.Size.Value);
				metaData.Set("pageSize", query.Size.Value);
			}

			// set the sort value, if any
			var sortString = query.Sorts.Aggregate(",", (current, sort) =>
			                                            {
			                                            	var s = sort.ToString();
			                                            	if (string.IsNullOrEmpty(s))
			                                            		return current;
			                                            	return current + ',' + sort;
			                                            }).Trim(',', ' ');
			if (!string.IsNullOrEmpty(sortString))
				metaData.Set("sort", sortString);

			// return the meta data
			return metaData;
		}
		/// <summary>
		/// Maps all the <paramref name="hits"/> into <see cref="Record"/>s.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="hits">The <see cref="Hit"/>s.</param>
		/// <returns>Returns the mapped <see cref="Record"/>s.</returns>
		private static IEnumerable<Record> MapRecords(IMansionContext context, SearchQuery query, IEnumerable<Hit> hits)
		{
			// loop over all the hits
			foreach (var hit in hits)
			{
				// create the record
				var record = new Record();

				// map all its properties
				MapProperties(context, query, hit, record);

				// initialize the record
				record.Initialize(context);

				// return the mapped record
				yield return record;
			}
		}
		/// <summary>
		/// Maps the properties from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="source">The <see cref="Hit"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		private static void MapProperties(IMansionContext context, SearchQuery query, Hit source, IPropertyBag target)
		{
			// find the type mapping for this type
			var typeMapping = query.IndexDefinition.FindTypeMapping(source.Type);

			// loop over all the properties
			var document = JObject.Parse(source.Source);
			foreach (var property in document.Properties())
			{
				// find the property mapping for this property
				PropertyMapping propertyMapping;
				if (!typeMapping.Properties.TryGetValue(property.Name, out propertyMapping))
				{
					// just write the property without mapping
					SimplePropertyMapping.Map(property, target);
					continue;
				}

				// map the value using the property mapping
				propertyMapping.Map(context, source, property, target);
			}
		}
		#endregion
		#region Private Fields
		private readonly ConnectionManager connectionManager;
		private readonly IEnumerable<IQueryComponentMapper> converters;
		private readonly IndexDefinitionResolver indexDefinitionResolver;
		#endregion
	}
}