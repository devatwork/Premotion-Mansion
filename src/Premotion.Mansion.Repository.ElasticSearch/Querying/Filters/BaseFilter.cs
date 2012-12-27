using Newtonsoft.Json;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Filters
{
	/// <summary>
	/// Base class for all filters.
	/// </summary>
	public abstract class BaseFilter
	{
		#region Nested type: BaseFilterConverter
		/// <summary>
		/// Converts <see cref="BaseFilter"/>s.
		/// </summary>
		protected abstract class BaseFilterConverter<TFilter> : BaseWriteConverter<TFilter> where TFilter : BaseFilter
		{
			#region Overrides of JsonConverter
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected virtual void WriteObjectContent(JsonWriter writer, TFilter value, JsonSerializer serializer)
			{
				// write cache instruction
				writer.WritePropertyName("_cache");
				writer.WriteValue(value.Cache);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="BaseFilter"/>.
		/// </summary>
		protected BaseFilter()
		{
			// Enable cache by default
			Cache = true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Get/Sets a flag indicating whether this filter can be cached or not.
		/// </summary>
		/// <remarks>Defaults to true.</remarks>
		public bool Cache { get; set; }
		#endregion
	}
}