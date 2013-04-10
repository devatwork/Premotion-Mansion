using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// A filter that matches on all documents.
	/// </summary>
	[JsonConverter(typeof (MatchAllConverter))]
	[JsonObject(MemberSerialization.OptIn)]
	public class MatchAllFilter : BaseFilter
	{
		#region Nested type: MatchAllConverter
		private class MatchAllConverter : BaseFilterConverter<MatchAllFilter>
		{
			#region Overrides of BaseConverter<MatchAllFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, MatchAllFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject();
				writer.WritePropertyName("match_all");
				writer.WriteStartObject();
				writer.WriteEndObject();
				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Constructors
		#endregion
	}
}