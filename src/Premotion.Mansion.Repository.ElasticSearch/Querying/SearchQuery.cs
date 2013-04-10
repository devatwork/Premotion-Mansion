using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.ElasticSearch.Connection;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Facets;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts;
using Premotion.Mansion.Repository.ElasticSearch.Schema;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Represents an ElasticSearch query.
	/// </summary>
	[JsonConverter(typeof (SearchQueryConverter))]
	public class SearchQuery
	{
		#region Nested type: SearchQueryConverter
		/// <summary>
		/// Converts <see cref="SearchQuery"/>s.
		/// </summary>
		private class SearchQueryConverter : BaseWriteConverter<SearchQuery>
		{
			#region Overrides of JsonConverter
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, SearchQuery value, JsonSerializer serializer)
			{
				writer.WriteStartObject();

				// write the filter
				List<BaseFilter> filterList;
				BaseFilter filter = null;
				if (value.filterListStack.TryPeek(out filterList))
				{
					writer.WritePropertyName("filter");
					if (filterList.Count == 1)
					{
						filter = filterList[0];
						serializer.Serialize(writer, filter);
					}
					else if (filterList.Count > 1)
					{
						filter = new AndFilter().Add(filterList);
						serializer.Serialize(writer, filter);
					}
				}

				// write the sort
				if (value.sortList.Count > 0)
				{
					writer.WritePropertyName("sort");
					writer.WriteStartArray();
					foreach (var sort in value.sortList)
						serializer.Serialize(writer, sort);
					writer.WriteEndArray();
				}

				// write the facets
				if (value.facetList.Count > 0)
				{
					writer.WritePropertyName("facets");
					writer.WriteStartObject();
					foreach (var facet in value.facetList)
					{
						if (filter != null)
							facet.Filter = filter;
						serializer.Serialize(writer, facet);
					}
					writer.WriteEndObject();
				}

				// write the query
				if (value.queryList.Count > 0)
				{
					writer.WritePropertyName("query");
					if (value.queryList.Count == 1)
					{
						var query = value.queryList[0];
						serializer.Serialize(writer, query);
					}
					else if (value.queryList.Count > 1)
					{
						var query = new BoolQuery().AddMust(value.queryList);
						serializer.Serialize(writer, query);
					}
				}

				// write paging options
				if (value.From.HasValue)
				{
					writer.WritePropertyName("from");
					writer.WriteValue(value.From);
				}
				if (value.Size.HasValue)
				{
					writer.WritePropertyName("size");
					writer.WriteValue(value.Size);
				}

				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a new search descriptor.
		/// </summary>
		/// <param name="indexDefinition">The <see cref="IndexDefinition"/> on which this search will be executed.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> on which this search will be executed.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public SearchQuery(IndexDefinition indexDefinition, TypeMapping typeMapping)
		{
			// validate arguments
			if (indexDefinition == null)
				throw new ArgumentNullException("indexDefinition");
			if (typeMapping == null)
				throw new ArgumentNullException("typeMapping");

			// set the values
			this.indexDefinition = indexDefinition;
			this.typeMapping = typeMapping;
			disposableChain.Add(filterListStack.Push(new List<BaseFilter>()));
			Size = 9999;
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the <paramref name="filter"/>.
		/// </summary>
		/// <param name="filter">The <see cref="BaseFilter"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filter"/> is null.</exception>
		public void Add(BaseFilter filter)
		{
			// validate arguments
			if (filter == null)
				throw new ArgumentNullException("filter");

			// add the filter
			List<BaseFilter> filterList;
			if (!FilterListStack.TryPeek(out filterList))
				throw new InvalidOperationException("No filter list found on the the filter stack");
			filterList.Add(filter);
		}
		/// <summary>
		/// Adds the <paramref name="sort"/>.
		/// </summary>
		/// <param name="sort">The <see cref="BaseSort"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sort"/> is null.</exception>
		public void Add(BaseSort sort)
		{
			// validate arguments
			if (sort == null)
				throw new ArgumentNullException("sort");

			// add the sort
			sortList.Add(sort);
		}
		/// <summary>
		/// Adds the <paramref name="facet"/>.
		/// </summary>
		/// <param name="facet">The <see cref="BaseFacet"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="facet"/> is null.</exception>
		public void Add(BaseFacet facet)
		{
			// validate arguments
			if (facet == null)
				throw new ArgumentNullException("facet");

			// add the sort
			facetList.Add(facet);
		}
		/// <summary>
		/// Adds the <paramref name="query"/>.
		/// </summary>
		/// <param name="query">The <see cref="BaseQuery"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
		public void Add(BaseQuery query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// add the sort
			queryList.Add(query);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="IndexDefinition"/> on which this search will be executed.
		/// </summary>
		public IndexDefinition IndexDefinition
		{
			get { return indexDefinition; }
		}
		/// <summary>
		/// Gets the <see cref="TypeMapping"/> on which this search will be executed.
		/// </summary>
		public TypeMapping TypeMapping
		{
			get { return typeMapping; }
		}
		/// <summary>
		/// The record from which to start returning results.
		/// </summary>
		/// <remarks>Defaults to 0.</remarks>
		public int? From { get; set; }
		/// <summary>
		/// The number of results which to get.
		/// </summary>
		/// <remarks>Defaults to 10</remarks>
		public int? Size { get; set; }
		/// <summary>
		/// Gets the <see cref="BaseSort"/> from this query.
		/// </summary>
		public IEnumerable<BaseSort> Sorts
		{
			get { return sortList; }
		}
		/// <summary>
		/// Gets the <see cref="BaseFacet"/>s.
		/// </summary>
		public IEnumerable<BaseFacet> Facets
		{
			get { return facetList; }
		}
		/// <summary>
		/// Gets the filter list stack.
		/// </summary>
		public IAutoPopStack<List<BaseFilter>> FilterListStack
		{
			get { return filterListStack; }
		}
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented, ElasicSearchJsonSerializerSettings.Settings);
		}
		#endregion
		#region Private Fields
		private readonly DisposableChain disposableChain = new DisposableChain();
		private readonly List<BaseFacet> facetList = new List<BaseFacet>();
		private readonly AutoPopStack<List<BaseFilter>> filterListStack = new AutoPopStack<List<BaseFilter>>();
		private readonly IndexDefinition indexDefinition;
		private readonly List<BaseQuery> queryList = new List<BaseQuery>();
		private readonly List<BaseSort> sortList = new List<BaseSort>();
		private readonly TypeMapping typeMapping;
		#endregion
	}
}