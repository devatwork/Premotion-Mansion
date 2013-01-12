using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts
{
	/// <summary>
	/// Implements the score sort.
	/// </summary>
	[JsonConverter(typeof (ScoreSortConverter))]
	public class ScoreSort : BaseSort
	{
		#region Nested type: ScoreSortConverter
		/// <summary>
		/// Converts <see cref="FieldSort"/>.
		/// </summary>
		private class ScoreSortConverter : BaseSortConverter<ScoreSort>
		{
			#region Overrides of BaseConverter<FieldSort>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, ScoreSort value, JsonSerializer serializer)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("_score");
				writer.WriteStartObject();
				writer.WriteEndObject();
				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Singleton Impl
		/// <summary>
		/// Static accessor.
		/// </summary>
		public static ScoreSort Instance = new ScoreSort();
		/// <summary>
		/// Use <see cref="Instance"/>.
		/// </summary>
		private ScoreSort()
		{
		}
		#endregion
	}
}