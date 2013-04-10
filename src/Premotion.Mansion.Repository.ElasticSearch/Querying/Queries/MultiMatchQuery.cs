using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Queries
{
	/// <summary>
	/// Multi match query.
	/// </summary>
	[JsonConverter(typeof (MultiMatchQueryConverter))]
	public class MultiMatchQuery : BaseQuery
	{
		#region Nested type: MultiMatchQueryConverter
		/// <summary>
		/// Converts <see cref="MultiMatchQuery"/>.
		/// </summary>
		private class MultiMatchQueryConverter : BaseWriteConverter<MultiMatchQuery>
		{
			#region Overrides of BaseWriteConverter<MultiMatchQuery>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, MultiMatchQuery value, JsonSerializer serializer)
			{
				writer.WriteStartObject(); // root
				writer.WritePropertyName("multi_match");
				writer.WriteStartObject(); // multi_match

				// write the fields
				writer.WritePropertyName("query");
				writer.WriteValue(value.query);

				// write the fields
				writer.WritePropertyName("fields");
				writer.WriteStartArray(); // fields
				foreach (var field in value.fields)
					writer.WriteValue(field);
				writer.WriteEndArray(); // fields

				writer.WriteEndObject(); // multi_match
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a multi match query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="fields">The fields on which to search.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public MultiMatchQuery(string query, IEnumerable<string> fields)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");
			if (fields == null)
				throw new ArgumentNullException("fields");

			// set the values
			this.query = query;
			this.fields = fields.ToArray();
		}
		#endregion
		#region Private Fields
		private readonly string[] fields;
		private readonly string query;
		#endregion
	}
}