using System;
using Newtonsoft.Json;
using Premotion.Mansion.Repository.ElasticSearch.Querying.Queries;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Wraps any query to be used as a filter. Can be placed within queries that accept a filter.
	/// </summary>
	[JsonConverter(typeof (QueryFilterConverter))]
	public class QueryFilter : BaseFilter
	{
		#region Nested type: QueryFilterConverter
		/// <summary>
		/// Converts <see cref="NestedFilter"/>.
		/// </summary>
		private class QueryFilterConverter : BaseFilterConverter<QueryFilter>
		{
			#region Overrides of BaseConverter<QueryFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, QueryFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject(); // root
				writer.WritePropertyName("fquery");
				writer.WriteStartObject(); // fquery

				writer.WritePropertyName("query");
				serializer.Serialize(writer, value.query);

				writer.WriteEndObject(); // fquery
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a query filter.
		/// </summary>
		/// <param name="query">The <see cref="BaseQuery"/> on which to filter.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public QueryFilter(BaseQuery query)
		{
			// validate arguments
			if (query == null)
				throw new ArgumentNullException("query");

			// set the values
			this.query = query;
		}
		#endregion
		#region Private Fields
		private readonly BaseQuery query;
		#endregion
	}
}