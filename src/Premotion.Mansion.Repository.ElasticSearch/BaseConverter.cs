using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Base class for all custom <see cref="JsonConverter"/>.
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	public abstract class BaseConverter<TType> : JsonConverter
	{
		#region Overrides of JsonConverter
		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
		public override sealed void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DoWriteJson(writer, (TType) value, serializer);
		}
		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
		protected abstract void DoWriteJson(JsonWriter writer, TType value, JsonSerializer serializer);
		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		public override sealed object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			return typeof (TType).IsAssignableFrom(objectType);
		}
		#endregion
	}
}