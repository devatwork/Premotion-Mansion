using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Provides custom serializers.
	/// </summary>
	public static class Serializers
	{
		/// <summary>
		/// Handles the conversion of guids properly.
		/// </summary>
		public class GuidConverter : JsonConverter
		{
			/// <summary>
			/// Determines whether this instance can convert the specified object type.
			/// </summary>
			/// <param name="objectType">Type of the object.</param>
			/// <returns>
			/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
			/// </returns>
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof (Guid);
			}
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("$type");
				writer.WriteValue(value.GetType().FullName);
				writer.WritePropertyName("$value");
				writer.WriteValue(value);
				writer.WriteEndObject();
			}
			/// <summary>
			/// Reads the JSON representation of the object.
			/// </summary>
			/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
			/// <param name="objectType">Type of the object.</param>
			/// <param name="existingValue">The existing value of object being read.</param>
			/// <param name="serializer">The calling serializer.</param>
			/// <returns>The object value.</returns>
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				return existingValue;
			}
		}
		/// <summary>
		/// Initializes the serializers.
		/// </summary>
		static Serializers()
		{
			JsonSerializer = new JsonSerializer {
				TypeNameHandling = TypeNameHandling.Auto
			};
		}
		/// <summary>
		/// The <see cref="JsonSerializer"/> to use.
		/// </summary>
		public static readonly JsonSerializer JsonSerializer;
	}
}