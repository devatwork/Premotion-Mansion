using System;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Range query.
	/// </summary>
	[JsonConverter(typeof (RangeFilterConverter))]
	public class RangeFilter : BaseFilter
	{
		#region Nested type: RangeFilterConverter
		private class RangeFilterConverter : BaseFilterConverter<RangeFilter>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, RangeFilter value, JsonSerializer serializer)
			{
				// writer the inner filter
				writer.WriteStartObject(); // root
				writer.WritePropertyName("range");
				writer.WriteStartObject(); // range

				writer.WritePropertyName(value.propertyName.ToLower());
				writer.WriteStartObject(); // property

				// write from, if any
				if (value.from != null)
				{
					writer.WritePropertyName("from");
					writer.WriteValue(value.from);
				}

				// write to, if any
				if (value.to != null)
				{
					writer.WritePropertyName("to");
					writer.WriteValue(value.to);
				}

				// write include_lower, if any
				if (value.includeLower.HasValue)
				{
					writer.WritePropertyName("include_lower");
					writer.WriteValue(value.includeLower.Value);
				}

				// write include_upper, if any
				if (value.includeUpper.HasValue)
				{
					writer.WritePropertyName("include_upper");
					writer.WriteValue(value.includeUpper.Value);
				}

				writer.WriteEndObject(); // property

				// write other
				WriteObjectContent(writer, value, serializer);

				writer.WriteEndObject(); // range
				writer.WriteEndObject(); // root
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a range filter.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to filter.</param>
		/// <param name="from">The lower bound. Defaults to start from the first.</param>
		/// <param name="to">The upper bound. Defaults to unbounded.</param>
		/// <param name="includeLower">Should the first from (if set) be inclusive or not. Defaults to true</param>
		/// <param name="includeUpper">Should the last to (if set) be inclusive or not. Defaults to true.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if both <paramref name="from"/> and <paramref name="to"/> are set.</exception>
		public RangeFilter(string propertyName, object from = null, object to = null, bool? includeLower = null, bool? includeUpper = null)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");
			if (from == null && to == null)
				throw new ArgumentException("Either from or to must be specified");

			// set the values
			this.propertyName = propertyName;
			this.from = from;
			this.to = to;
			this.includeLower = includeLower;
			this.includeUpper = includeUpper;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a less than <see cref="RangeFilter"/> for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to check.</param>
		/// <param name="value">The value.</param>
		/// <returns>The created <see cref="RangeFilter"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public static RangeFilter LessThan(string propertyName, object value)
		{
			return new RangeFilter(propertyName, to: value, includeUpper: false);
		}
		/// <summary>
		/// Constructs a less than or equal to <see cref="RangeFilter"/> for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to check.</param>
		/// <param name="value">The value.</param>
		/// <returns>The created <see cref="RangeFilter"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public static RangeFilter LessThanOrEqualTo(string propertyName, object value)
		{
			return new RangeFilter(propertyName, to: value, includeUpper: true);
		}
		/// <summary>
		/// Constructs a greater than <see cref="RangeFilter"/> for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to check.</param>
		/// <param name="value">The value.</param>
		/// <returns>The created <see cref="RangeFilter"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public static RangeFilter GreaterThan(string propertyName, object value)
		{
			return new RangeFilter(propertyName, from: value, includeUpper: false);
		}
		/// <summary>
		/// Constructs a greater than or equal to <see cref="RangeFilter"/> for the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property on which to check.</param>
		/// <param name="value">The value.</param>
		/// <returns>The created <see cref="RangeFilter"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public static RangeFilter GreaterThanOrEqualTo(string propertyName, object value)
		{
			return new RangeFilter(propertyName, from: value, includeUpper: true);
		}
		#endregion
		#region Private Fields
		private readonly object from;
		private readonly bool? includeLower;
		private readonly bool? includeUpper;
		private readonly string propertyName;
		private readonly object to;
		#endregion
	}
}