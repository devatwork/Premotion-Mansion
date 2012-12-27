using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Queries
{
	/// <summary>
	/// The text_phrase_prefix is the same as text_phrase, expect it allows for prefix matches on the last term in the text.
	/// </summary>
	[JsonConverter(typeof (TextPhrasePrefixQueryConverter))]
	public class TextPhrasePrefixQuery : BaseQuery
	{
		#region Nested type: TextPhrasePrefixQueryConverter
		/// <summary>
		/// Converts <see cref="TextPhrasePrefixQuery"/>.
		/// </summary>
		private class TextPhrasePrefixQueryConverter : BaseWriteConverter<TextPhrasePrefixQuery>
		{
			#region Overrides of BaseWriteConverter<MultiMatchQuery>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, TextPhrasePrefixQuery value, JsonSerializer serializer)
			{
				writer.WritePropertyName("text_phrase_prefix");
				writer.WriteStartObject(); // text_phrase_prefix

				// write the field
				writer.WritePropertyName(value.field);
				writer.WriteStartObject(); // field

				// write the query
				writer.WritePropertyName("query");
				writer.WriteValue(value.query);

				writer.WriteEndObject(); // field
				writer.WriteEndObject(); // text_phrase_prefix
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a multi match query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="field">The fields on which to search.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public TextPhrasePrefixQuery(string query, string field)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");
			if (field == null)
				throw new ArgumentNullException("field");

			// set the values
			this.query = query;
			this.field = field;
		}
		#endregion
		#region Private Fields
		private readonly string field;
		private readonly string query;
		#endregion
	}
}