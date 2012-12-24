using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Implements the mapping for the pointer property.
	/// </summary>
	[JsonConverter(typeof (TreeRelationsPropertyMappingConverter))]
	public class TreeRelationsPropertyMapping : PropertyMapping
	{
		#region Nested type: TreeRelationsPropertyMappingConverter
		/// <summary>
		/// Maps <see cref="TreeRelationsPropertyMapping"/>
		/// </summary>
		private class TreeRelationsPropertyMappingConverter : BaseWriteConverter<TreeRelationsPropertyMapping>
		{
			#region Overrides of BaseConverter<TreeRelationsPropertyMapping>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, TreeRelationsPropertyMapping value, JsonSerializer serializer)
			{
				writer.WritePropertyName("type");
				writer.WriteValue("nested");

				writer.WritePropertyName("properties");
				writer.WriteStartObject(); // propertise

				// pointer
				writer.WritePropertyName("pointer");
				writer.WriteStartObject();
				writer.WritePropertyName("type");
				writer.WriteValue("integer");
				writer.WriteEndObject();

				// path
				writer.WritePropertyName("path");
				writer.WriteStartObject();
				writer.WritePropertyName("type");
				writer.WriteValue("string");
				writer.WritePropertyName("index");
				writer.WriteValue("not_analyzed");
				writer.WriteEndObject();

				// structure
				writer.WritePropertyName("structure");
				writer.WriteStartObject();
				writer.WritePropertyName("type");
				writer.WriteValue("string");
				writer.WritePropertyName("index");
				writer.WriteValue("not_analyzed");
				writer.WriteEndObject();

				writer.WriteEndObject(); // properties
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the property mapping.
		/// </summary>
		public TreeRelationsPropertyMapping() : base("pointer")
		{
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
			document.Add(Field, source.Get<NodePointer>(context, "pointer"));
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
			// null check
			if (property.Value == null)
				return;

			// get the arrays
			var jObject = (JObject) property.Value;
			var jPath = (JArray) jObject.Property("path").Value;
			var jPointer = (JArray) jObject.Property("pointer").Value;
			var jStructure = (JArray) jObject.Property("structure").Value;

			// turn them into an array
			var path = jPath.Select(value => value.Value<string>()).ToArray();
			var pointer = jPointer.Select(value => value.Value<int>()).ToArray();
			var structure = jStructure.Select(value => value.Value<string>()).ToArray();

			// create the pointer
			target.Set("pointer", new NodePointer(pointer, structure, path));
		}
		#endregion
	}
}