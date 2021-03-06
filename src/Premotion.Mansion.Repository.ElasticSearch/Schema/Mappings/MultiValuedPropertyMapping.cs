using System;
using System.Collections.Generic;
using System.Linq;
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
	public class MultiValuedPropertyMapping : SinglePropertyMapping
	{
		#region Nested type: MultiValuedPropertyMappingDescriptor
		/// <summary>
		/// Represents the <see cref="TypeDescriptor"/> for multi-valued properties.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "multiValuedProperty")]
		public class MultiValuedPropertyMappingDescriptor : SinglePropertyMappingDescriptor
		{
			#region Create Methods
			/// <summary>
			/// Constructs a <see cref="MultiValuedPropertyMappingDescriptor"/>.
			/// </summary>
			/// <param name="expressionScriptService">The <see cref="IExpressionScriptService"/></param>
			public MultiValuedPropertyMappingDescriptor(IExpressionScriptService expressionScriptService) : base(expressionScriptService)
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
				return new MultiValuedPropertyMapping(property) {
					// map the type
					Type = Properties.Get<string>(context, "type")
				};
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="property"/>.
		/// </summary>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public MultiValuedPropertyMapping(IPropertyDefinition property) : base(property.Name)
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
			// get the property value
			var raw = source.Get(context, Field, string.Empty) ?? string.Empty;

			// split on comma, trim all values, remove empty entries
			var values = raw.Split(new[] {','}).Select(x => Normalize(context, x.Trim()).ToString()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

			// write the values to the document
			document.Add(Field, values);
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

			// expect an array
			var values = (JArray) property.Value;

			target.Set(property.Name, string.Join(",", values.Select(value => value.Value<string>()).ToArray()));
		}
		#endregion
	}
}