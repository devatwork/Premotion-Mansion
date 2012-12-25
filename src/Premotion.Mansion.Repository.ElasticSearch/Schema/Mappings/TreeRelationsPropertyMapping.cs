using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
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
		#region Nested type: TreeRelationsPropertyMappingDescriptor
		/// <summary>
		/// Represents the <see cref="TypeDescriptor"/> for elastic search tree relation properties.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "treeRelations")]
		public class TreeRelationsPropertyMappingDescriptor : PropertyMappingBaseDescriptor
		{
			#region Overrides of PropertyMappingBaseDescriptor
			/// <summary>
			/// Adds <see cref="PropertyMapping"/>s of <paramref name="property"/> to the given <paramref name="typeMapping"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="property">The <see cref="IPropertyDefinition"/> of the property for which to add the <see cref="PropertyMapping"/>s.</param>
			/// <param name="typeMapping">The <see cref="TypeMapping"/> to which to add the new <see cref="PropertyMapping"/>s.</param>
			protected override void DoAddMappingTo(IMansionContext context, IPropertyDefinition property, TypeMapping typeMapping)
			{
				typeMapping.Add(new TreeRelationsPropertyMapping());
				typeMapping.Add(new MultiFieldPropertyMapping(
				                	"name",
				                	"name",
				                	"untouched",
				                	new[]
				                	{
				                		new SingleValuedPropertyMapping("name")
				                		{
				                			Type = "string",
				                			Index = "analyzed",
				                			Boost = 5
				                		},
				                		new SingleValuedPropertyMapping("untouched")
				                		{
				                			Type = "string",
				                			Index = "not_analyzed"
				                		}
				                	}
				                	));
				typeMapping.Add(new SingleValuedPropertyMapping("type")
				                {
				                	Type = "string",
				                	Index = "not_analyzed"
				                });
				typeMapping.Add(new SingleValuedPropertyMapping("order")
				                {
				                	Type = "long"
				                });
				typeMapping.Add(new IgnoredPropertyMapping("depth"));
				typeMapping.Add(new IgnoredPropertyMapping("structure"));
				typeMapping.Add(new IgnoredPropertyMapping("path"));
				typeMapping.Add(new IgnoredPropertyMapping("parentId"));
				typeMapping.Add(new IgnoredPropertyMapping("parentPointer"));
				typeMapping.Add(new IgnoredPropertyMapping("parentPath"));
				typeMapping.Add(new IgnoredPropertyMapping("parentStructure"));
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