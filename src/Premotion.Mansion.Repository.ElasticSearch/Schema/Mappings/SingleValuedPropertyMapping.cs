using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
{
	/// <summary>
	/// Maps a simple property.
	/// </summary>
	public class SingleValuedPropertyMapping : SinglePropertyMapping
	{
		#region Nested type: SingleValuedPropertyMappingDescriptor
		/// <summary>
		/// Represents the <see cref="TypeDescriptor"/> for elastic search properties.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "singleValuedProperty")]
		public class SingleValuedPropertyMappingDescriptor : SinglePropertyMappingDescriptor
		{
			#region Create Methods
			/// <summary>
			/// Constructs a <see cref="SingleValuedPropertyMappingDescriptor"/>.
			/// </summary>
			/// <param name="expressionScriptService">The <see cref="IExpressionScriptService"/></param>
			public SingleValuedPropertyMappingDescriptor(IExpressionScriptService expressionScriptService) : base(expressionScriptService)
			{
			}
			/// <summary>
			/// Creates a <see cref="PropertyMapping"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
			/// <returns>The created <see cref="PropertyMapping"/>.</returns>
			protected override SinglePropertyMapping DoCreateSingleMapping(IMansionContext context, IPropertyDefinition property)
			{
				// create the mapping
				return new SingleValuedPropertyMapping(property.Name) {
					// map the type
					Type = Properties.Get<string>(context, "type")
				};
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="field"/>.
		/// </summary>
		/// <param name="field">The name of the property mapped by this mapper.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public SingleValuedPropertyMapping(string field) : base(field)
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
			// just map the value to the document
			document.Add(Field, Normalize(context, source.Get<object>(context, Field)));
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
			Map(property, target);
		}
		/// <summary>
		/// Maps the given <paramref name="property"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="property">The <see cref="JProperty"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		public static void Map(JProperty property, IPropertyBag target)
		{
			// validate arguments
			if (property == null)
				throw new ArgumentNullException("property");
			if (target == null)
				throw new ArgumentNullException("target");

			// check for null
			if (property.Value == null)
				return;

			// get the correct type value
			switch (property.Value.Type)
			{
				case JTokenType.Boolean:
				{
					target.Set(property.Name, property.Value.Value<bool>());
				}
					break;
				case JTokenType.Date:
				{
					target.Set(property.Name, property.Value.Value<DateTime>());
				}
					break;
				case JTokenType.Float:
				{
					target.Set(property.Name, property.Value.Value<float>());
				}
					break;
				case JTokenType.Guid:
				{
					target.Set(property.Name, property.Value.Value<Guid>());
				}
					break;
				case JTokenType.Integer:
				{
					target.Set(property.Name, property.Value.Value<int>());
				}
					break;
				case JTokenType.Null:
				{
					// do not map
				}
					break;
				case JTokenType.String:
				{
					target.Set(property.Name, property.Value.Value<string>());
				}
					break;
				case JTokenType.TimeSpan:
				{
					target.Set(property.Name, property.Value.Value<TimeSpan>());
				}
					break;
				case JTokenType.Uri:
				{
					target.Set(property.Name, property.Value.Value<Uri>());
				}
					break;
				default:
					throw new NotSupportedException(string.Format("Can not map property '{0}' of type '{1}' with value '{2}'", property.Name, property.Type, property.Value));
			}
		}
		#endregion
	}
}