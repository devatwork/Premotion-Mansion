using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the mapping of a property.
	/// </summary>
	public abstract class PropertyMapping
	{
		#region Constructors
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="property"/>.
		/// </summary>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected PropertyMapping(IPropertyDefinition property) : this(property.Name)
		{
		}
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="propertyName"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property mapped by this mapper.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected PropertyMapping(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set value
			Name = propertyName.ToLower();
		}
		#endregion
		#region Transform Methods
		/// <summary>
		/// Transforms the given <paramref name="source"/> into an ElasticSearch document.
		/// </summary>
		/// <param name="context">the <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="IPropertyBag"/> which to transform.</param>
		/// <param name="document">The document to which to write the mapped value.</param>
		/// <returns>Returns the resulting document.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Transform(IMansionContext context, IPropertyBag source, Dictionary<string, object> document)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (document == null)
				throw new ArgumentNullException("document");

			// invoke template method
			DoTransform(context, source, document);
		}
		/// <summary>
		/// Transforms the given <paramref name="source"/> into an ElasticSearch document.
		/// </summary>
		/// <param name="context">the <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="IPropertyBag"/> which to transform.</param>
		/// <param name="document">The document to which to write the mapped value.</param>
		/// <returns>Returns the resulting document.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected abstract void DoTransform(IMansionContext context, IPropertyBag source, Dictionary<string, object> document);
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps the properties from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="Hit"/>.</param>
		/// <param name="property">The <see cref="JProperty"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		public void Map(IMansionContext context, Hit source, JProperty property, IPropertyBag target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (property == null)
				throw new ArgumentNullException("property");
			if (target == null)
				throw new ArgumentNullException("target");

			// invoke template method
			DoMap(context, source, property, target);
		}
		/// <summary>
		/// Maps the properties from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="Hit"/>.</param>
		/// <param name="property">The <see cref="JProperty"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		protected abstract void DoMap(IMansionContext context, Hit source, JProperty property, IPropertyBag target);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		[JsonIgnore]
		public string Name { get; private set; }
		#endregion
	}
}