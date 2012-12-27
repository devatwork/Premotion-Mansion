using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements a term filter.
	/// </summary>
	[JsonConverter(typeof (TermFilterConverter))]
	[JsonObject(MemberSerialization.OptIn)]
	public class TermFilter : BaseFilter
	{
		#region Nested type: TermFilterConverter
		private class TermFilterConverter : BaseFilterConverter<TermFilter>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, TermFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject();
				writer.WritePropertyName("term");
				writer.WriteStartObject();
				writer.WritePropertyName(value.propertyName.ToLower());
				serializer.Serialize(writer, value.value);
				WriteObjectContent(writer, value, serializer);
				writer.WriteEndObject();
				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the term filter.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to filter.</param>
		/// <param name="value">The value the property should have.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public TermFilter(string propertyName, object value)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (value == null)
				throw new ArgumentNullException("value");

			// set value
			this.propertyName = propertyName;
			this.value = value;
		}
		#endregion
		#region Private Fields
		private readonly string propertyName;
		private readonly object value;
		#endregion
	}
}