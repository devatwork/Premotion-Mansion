using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements the nested filter.
	/// </summary>
	[JsonConverter(typeof (NestedFilterConverter))]
	public class NestedFilter : BaseFilter
	{
		#region Nested type: NestedFilterConverter
		/// <summary>
		/// Converts <see cref="NestedFilter"/>.
		/// </summary>
		private class NestedFilterConverter : BaseFilterConverter<NestedFilter>
		{
			#region Overrides of BaseConverter<NestedFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, NestedFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject(); // root
				writer.WritePropertyName("nested");
				writer.WriteStartObject(); // filter

				// write the path
				writer.WritePropertyName("path");
				writer.WriteValue(value.path);

				// write the query
				writer.WritePropertyName("query");
				writer.WriteStartObject(); // query
				writer.WritePropertyName("filtered");
				writer.WriteStartObject(); // filtered

				// write match all query
				writer.WritePropertyName("query");
				writer.WriteStartObject(); // query
				writer.WritePropertyName("match_all");
				writer.WriteStartObject(); // match_all
				writer.WriteEndObject(); // match_all
				writer.WriteEndObject(); // query

				// writer filter
				writer.WritePropertyName("filter");
				serializer.Serialize(writer, value.filter);

				writer.WriteEndObject(); // filtered
				writer.WriteEndObject(); // query

				// write other
				WriteObjectContent(writer, value, serializer);

				writer.WriteEndObject(); // filter
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a range filter.
		/// </summary>
		/// <param name="path">The name of the property on which to filter.</param>
		/// <param name="filter">The <see cref="BaseFilter"/> which will be applied..</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public NestedFilter(string path, BaseFilter filter)
		{
			// validate arguments
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");
			if (filter == null)
				throw new ArgumentNullException("filter");

			// set the values
			this.path = path;
			this.filter = filter;
			Cache = filter.Cache;
		}
		#endregion
		#region Private Fields
		private readonly BaseFilter filter;
		private readonly string path;
		#endregion
	}
}