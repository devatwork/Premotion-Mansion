using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements the Not <see cref="BaseFilter"/>.
	/// </summary>
	[JsonConverter(typeof (NotFilterConverter))]
	public class NotFilter : BaseFilter
	{
		#region Nested type: NotFilterConverter
		/// <summary>
		/// Converts <see cref="NotFilter"/>.
		/// </summary>
		private class NotFilterConverter : BaseFilterConverter<NotFilter>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, NotFilter value, JsonSerializer serializer)
			{
				writer.WriteStartObject(); // root

				// writer the operation
				writer.WritePropertyName("not");
				writer.WriteStartObject(); // not
				writer.WritePropertyName("filter");
				serializer.Serialize(writer, value.filter);

				// write other content
				WriteObjectContent(writer, value, serializer);

				writer.WriteEndObject(); // not
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a not filter.
		/// </summary>
		/// <param name="filter">The <see cref="BaseFilter"/> being negated.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filter"/> is null.</exception>
		public NotFilter(BaseFilter filter)
		{
			// validate arguments
			if (filter == null)
				throw new ArgumentNullException("filter");

			// set the value
			this.filter = filter;
		}
		#endregion
		#region Private Fields
		private readonly BaseFilter filter;
		#endregion
	}
}