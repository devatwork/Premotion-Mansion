using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Maps a simple property.
	/// </summary>
	public class MultiValuedPropertyMapping : SinglePropertyMapping
	{
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="property"/>.
		/// </summary>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public MultiValuedPropertyMapping(IPropertyDefinition property) : base(property)
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
			var raw = source.Get(context, Name, string.Empty) ?? string.Empty;

			// split on comma, trim all values, remove empty entries
			var values = raw.Split(new[] {','}).Select(x => Normalize(x.Trim()).ToString()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

			// write the values to the document
			document.Add(Name, values);
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