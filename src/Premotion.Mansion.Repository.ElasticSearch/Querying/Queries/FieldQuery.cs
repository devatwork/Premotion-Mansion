using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Queries
{
	/// <summary>
	/// A query that executes a query string against a specific field. It is a simplified version of query_string query (by setting the default_field to the field this query executed against). In its simplest form:
	/// </summary>
	[JsonConverter(typeof (FieldQueryConverter))]
	public class FieldQuery : BaseQuery
	{
		#region Nested type: FieldQueryConverter
		/// <summary>
		/// Converts <see cref="TextPhrasePrefixQuery"/>.
		/// </summary>
		private class FieldQueryConverter : BaseWriteConverter<FieldQuery>
		{
			#region Overrides of BaseWriteConverter<FieldQuery>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, FieldQuery value, JsonSerializer serializer)
			{
				writer.WritePropertyName("field");
				writer.WriteStartObject(); // field

				// write the field name
				writer.WritePropertyName(value.field);
				writer.WriteStartObject(); // field

				// write the query
				writer.WritePropertyName("query");
				writer.WriteValue(value.query);

				writer.WriteEndObject(); // field

				writer.WriteEndObject(); // field
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a field query.
		/// </summary>
		/// <param name="field">The fields on which to search.</param>
		/// <param name="query">The query.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public FieldQuery(string field, object query)
		{
			// validate arguments
			if (field == null)
				throw new ArgumentNullException("field");
			if (query == null)
				throw new ArgumentNullException("query");

			// set the values
			this.query = query;
			this.field = field;
		}
		#endregion
		#region Private Fields
		private readonly string field;
		private readonly object query;
		#endregion
	}
}