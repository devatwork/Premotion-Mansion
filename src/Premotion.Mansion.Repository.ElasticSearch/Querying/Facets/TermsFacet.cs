using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Data.Facets;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying.Facets
{
	/// <summary>
	/// Implements a terms facet. Allow to specify field facets that return the N most frequent terms.
	/// </summary>
	[JsonConverter(typeof (TermsFacetConverter))]
	public class TermsFacet : BaseFacet
	{
		#region Nested type: TermsFacetConverter
		/// <summary>
		/// Converts <see cref="TermsFacet"/>.
		/// </summary>
		private class TermsFacetConverter : BaseFacetConverter<TermsFacet>
		{
			#region Overrides of BaseConverter<CompositeFilter>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, TermsFacet value, JsonSerializer serializer)
			{
				writer.WritePropertyName(value.Definition.FriendlyName.ToLower());
				writer.WriteStartObject(); // field

				writer.WritePropertyName("terms");
				writer.WriteStartObject(); // terms

				// write field
				writer.WritePropertyName("field");
				writer.WriteValue(value.Definition.PropertyName.ToLower());

				// write order
				if (!string.IsNullOrEmpty(value.order))
				{
					writer.WritePropertyName("order");
					writer.WriteValue(value.order);
				}

				// write size
				if (value.size.HasValue)
				{
					writer.WritePropertyName("size");
					writer.WriteValue(value.size.Value);
				}

				writer.WriteEndObject(); // terms

				// write facet filter
				WriteFacetFilter(writer, value, serializer);

				writer.WriteEndObject(); // field
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="TermsFacet"/>.
		/// </summary>
		/// <param name="definition">The <see cref="FacetDefinition"/>.</param>
		/// <param name="order">Allow to control the ordering of the terms facets, to be ordered by count, term, reverse_count or reverse_term. The default is count.</param>
		/// <param name="size">The number of values which to return. The default is 10.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="definition"/> is null.</exception>
		public TermsFacet(FacetDefinition definition, string order = null, int? size = null) : base(definition)
		{
			// set the values
			this.order = order;
			this.size = size;
		}
		#endregion
		#region Private Fields
		private readonly string order;
		private readonly int? size;
		#endregion
	}
}