using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Implements a terms filter.
	/// </summary>
	[JsonConverter(typeof (TermsFilterConverter))]
	[JsonObject(MemberSerialization.OptIn)]
	public class TermsFilter : BaseFilter
	{
		#region Nested type: TermsFilterConverter
		private class TermsFilterConverter : BaseFilterConverter<TermsFilter>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, TermsFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject();
				writer.WritePropertyName("terms");
				writer.WriteStartObject();
				writer.WritePropertyName(value.propertyName.ToLower());
				serializer.Serialize(writer, value.values);
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
		/// <param name="values">The values the property should have.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public TermsFilter(string propertyName, IEnumerable<object> values)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (values == null)
				throw new ArgumentNullException("values");

			// set value
			this.propertyName = propertyName;
			this.values = values.ToArray();
		}
		#endregion
		#region Private Fields
		private readonly string propertyName;
		private readonly object[] values;
		#endregion
	}
}