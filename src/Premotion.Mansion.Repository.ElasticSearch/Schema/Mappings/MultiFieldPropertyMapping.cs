using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
{
	/// <summary>
	/// Implements the field mapping of multifield properties.
	/// </summary>
	[JsonConverter(typeof (MultiFieldPropertyMappingConverter))]
	public class MultiFieldPropertyMapping : PropertyMapping
	{
		#region Nested type: MultiFieldPropertyMappingConverter
		/// <summary>
		/// Maps <see cref="TreeRelationsPropertyMapping"/>
		/// </summary>
		private class MultiFieldPropertyMappingConverter : BaseWriteConverter<MultiFieldPropertyMapping>
		{
			#region Overrides of BaseConverter<MultiFieldPropertyMapping>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, MultiFieldPropertyMapping value, JsonSerializer serializer)
			{
				writer.WritePropertyName("type");
				writer.WriteValue("multi_field");

				writer.WritePropertyName("fields");
				writer.WriteStartObject(); // fields
				foreach (var mapping in value.mappings)
				{
					writer.WritePropertyName(mapping.Field);
					writer.WriteStartObject(); // field
					serializer.Serialize(writer, mapping);
					writer.WriteEndObject(); // field
				}
				writer.WriteEndObject(); // fields
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given field.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public MultiFieldPropertyMapping(string field, string queryField, string sortField, IEnumerable<PropertyMapping> mappings) : base(field, queryField, sortField)
		{
			// validate arguments
			if (mappings == null)
				throw new ArgumentNullException("mappings");

			// set the values
			this.mappings = mappings.ToArray();
		}
		#endregion
		#region Overrides of PropertyMapping
		/// <summary>
		/// Transforms the given <paramref name="source"/> into an ElasticSearch document.
		/// </summary>
		/// <param name="context">the <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="IPropertyBag"/> which to transform.</param>
		/// <param name="document">The document to which to write the mapped value.</param>
		/// <returns>Returns the resulting document.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected override void DoTransform(IMansionContext context, IPropertyBag source, Dictionary<string, object> document)
		{
			mappings.First().Transform(context, source, document);
		}
		/// <summary>
		/// Maps the properties from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="Hit"/>.</param>
		/// <param name="property">The <see cref="JProperty"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		protected override void DoMap(IMansionContext context, Hit source, JProperty property, IPropertyBag target)
		{
			mappings.First().Map(context, source, property, target);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="PropertyMapping"/>s defined on this multifield  property mapping.
		/// </summary>
		public IEnumerable<PropertyMapping> Mappings
		{
			get { return mappings; }
		}
		#endregion
		#region Private Fields
		private readonly IEnumerable<PropertyMapping> mappings;
		#endregion
	}
}