using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Filters;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts;
using Premotion.Mansion.Repository.ElasticSearch.Schema;

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
		private class SearchQueryConverter : BaseConverter<SearchQuery>
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
				if (value.filterList.Count == 1)
				{
					writer.WritePropertyName("filter");
					serializer.Serialize(writer, value.filterList[0]);
				}
				else if (value.filterList.Count > 1)
				{
					writer.WritePropertyName("filter");
					serializer.Serialize(writer, new AndFilter().Add(value.filterList));
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
		}
		#endregion
		#region Filter Methods
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
		#endregion
		#region Private Fields
		private readonly List<BaseFilter> filterList = new List<BaseFilter>();
		private readonly IndexDefinition indexDefinition;
		private readonly List<BaseSort> sortList = new List<BaseSort>();
		private readonly TypeMapping typeMapping;
		#endregion
	}
}