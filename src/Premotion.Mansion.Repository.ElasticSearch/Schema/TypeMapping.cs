using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the mapping of a type.
	/// </summary>
	public class TypeMapping
	{
		#region Constructors
		/// <summary>
		/// Constucts a type mapping for the given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public TypeMapping(ITypeDefinition type)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");

			// set values
			Name = type.Name.ToLower();
			Source = new TypeMappingSource();
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds the given <paramref name="propertyMapping"/> to this mapping.
		/// </summary>
		/// <param name="propertyMapping">The <see cref="PropertyMapping"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the paramters is null.</exception>
		public void Add(PropertyMapping propertyMapping)
		{
			// validate arguments
			if (propertyMapping == null)
				throw new ArgumentNullException("propertyMapping");

			// if the property is already mapped, replace it
			if (propertyMappings.ContainsKey(propertyMapping.Name))
				propertyMappings.Remove(propertyMapping.Name);

			// add the property mapping
			propertyMappings.Add(propertyMapping.Name, propertyMapping);
		}
		#endregion
		#region Clone Methods
		/// <summary>
		/// Clones this type mapping for a new <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <paramref name="type"/>.</param>
		/// <returns>Returns the cloned type mapping.</returns>
		public TypeMapping Clone(ITypeDefinition type)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");

			// create a new mapping for the type
			var cloned = new TypeMapping(type);

			// copy all the property mappings
			foreach (var propertyMapping in propertyMappings)
				cloned.propertyMappings.Add(propertyMapping.Key, propertyMapping.Value);

			// return the clone
			return cloned;
		}
		#endregion
		#region Transform Methods
		/// <summary>
		/// Transforms the given <paramref name="source"/> into an ElasticSearch document.
		/// </summary>
		/// <param name="context">the <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="IPropertyBag"/> which to transform.</param>
		/// <returns>Returns the resulting document.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public IDictionary<string, object> Transform(IMansionContext context, IPropertyBag source)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");

			// create a new dictionary which represents the document 
			var document = new Dictionary<string, object>();

			// loop over all the properties
			foreach (var propertyName in source)
			{
				// ElasticSearch is case sensitive, use lower case property name
				var key = propertyName.Key.ToLower();

				// check if there is mapping defined for this property
				PropertyMapping mapping;
				if (propertyMappings.TryGetValue(key, out mapping))
				{
					// allow the property mapping to transorm the value
					mapping.Transform(context, source, document);
				}
				else
				{
					// just store the value in the document and let ElasticSearch figure out how to index it
					document.Add(key, propertyName.Value);
				}
			}

			// return the transformed document
			return document;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this type.
		/// </summary>
		[JsonIgnore]
		public string Name { get; private set; }
		/// <summary>
		/// The <see cref="TypeMappingSource"/>.
		/// </summary>
		[JsonProperty("_source")]
		public TypeMappingSource Source { get; set; }
		/// <summary>
		/// Getsthe <see cref="PropertyMapping"/>s of this mapping.
		/// </summary>
		[JsonProperty("properties")]
		public IDictionary<string, PropertyMapping> Properties
		{
			get { return propertyMappings; }
		}
		#endregion
		#region Private Fields
		private readonly Dictionary<string, PropertyMapping> propertyMappings = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}