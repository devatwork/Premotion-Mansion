using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Base class for all the composite <see cref="BaseFilter"/>.
	/// </summary>
	[JsonConverter(typeof (CompositeFilterConverter))]
	public abstract class CompositeFilter : BaseFilter
	{
		#region Nested type: CompositeFilterConverter
		private class CompositeFilterConverter : BaseFilterConverter<CompositeFilter>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, CompositeFilter value, JsonSerializer serializer)
			{
				writer.WriteStartObject();

				// writer the inner filter
				writer.WritePropertyName(value.op);
				writer.WriteStartArray();
				foreach (var filter in value.filterList)
					serializer.Serialize(writer, filter);
				writer.WriteEndArray();

				WriteObjectContent(writer, value, serializer);
				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the composite filter.
		/// </summary>
		/// <param name="op">The operation name.</param>
		protected CompositeFilter(string op)
		{
			// validate arguments
			if (string.IsNullOrEmpty(op))
				throw new ArgumentNullException("op");

			// set the value
			this.op = op;
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds a <see cref="BaseFilter"/> to this composite filter.
		/// </summary>
		/// <param name="filters">The <see cref="BaseFilter"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filters"/> is null.</exception>
		public void Add(params BaseFilter[] filters)
		{
			Add((IEnumerable<BaseFilter>) filters);
		}
		/// <summary>
		/// Adds a <see cref="BaseFilter"/> to this composite filter.
		/// </summary>
		/// <param name="filters">The <see cref="BaseFilter"/> which to add.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filters"/> is null.</exception>
		public void Add(IEnumerable<BaseFilter> filters)
		{
			// validate arguments
			if (filters == null)
				throw new ArgumentNullException("filters");

			//  add to the list
			filterList.AddRange(filters);
		}
		#endregion
		#region Private Fields
		private readonly List<BaseFilter> filterList = new List<BaseFilter>();
		private readonly string op;
		#endregion
	}
}