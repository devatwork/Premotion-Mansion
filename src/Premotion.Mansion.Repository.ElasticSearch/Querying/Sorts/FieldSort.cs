using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Sorts
{
	/// <summary>
	/// Implements the field sort.
	/// </summary>
	[JsonConverter(typeof (FieldSortConverter))]
	public class FieldSort : BaseSort
	{
		#region Nested type: FieldSortConverter
		/// <summary>
		/// Converts <see cref="FieldSort"/>.
		/// </summary>
		private class FieldSortConverter : BaseSortConverter<FieldSort>
		{
			#region Overrides of BaseConverter<FieldSort>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, FieldSort value, JsonSerializer serializer)
			{
				writer.WriteStartObject();
				writer.WritePropertyName(value.sort.PropertyName.ToLower());
				writer.WriteStartObject();
				writer.WritePropertyName("order");
				writer.WriteValue(value.sort.Ascending ? "asc" : "desc");
				writer.WriteEndObject();
				writer.WriteEndObject();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a field sort.
		/// </summary>
		/// <param name="sort">The <see cref="Sort"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="sort"/> is null.</exception>
		public FieldSort(Sort sort)
		{
			// validate arguments
			if (sort == null)
				throw new ArgumentNullException("sort");

			// validate arguments
			this.sort = sort;
		}
		#endregion
		#region Private Fields
		private readonly Sort sort;
		#endregion
	}
}