using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Querying;
using Premotion.Mansion.Repository.ElasticSearch.Responses;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Represents the mapping of a type.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
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
		#region Hit Map Methods
		/// <summary>
		/// Maps the given <paramref name="response"/> into a <see cref="RecordSet"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="response">The <see cref="SearchResponse"/>.</param>
		/// <returns>Returns the mapped record set.</returns>
		public static RecordSet MapRecordSet(IMansionContext context, SearchQuery query, SearchResponse response)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			if (response == null)
				throw new ArgumentNullException("response");

			// map all the hits
			var records = MapRecords(context, query, response.Hits.Hits);

			// map the set metadata
			var metaData = MapRecordSetMetaData(query, response.Hits);

			// create and return the set
			return new RecordSet(context, metaData, records);
		}
		/// <summary>
		/// Maps the meta data of the <paramref name="hits"/>.
		/// </summary>
		/// <param name="query">The source <see cref="SearchQuery"/>.</param>
		/// <param name="hits">The resulting <see cref="HitMetaData"/>.</param>
		/// <returns>Returns a <see cref="IPropertyBag"/> containing the meta data.</returns>
		private static IPropertyBag MapRecordSetMetaData(SearchQuery query, HitMetaData hits)
		{
			// create the meta data
			var metaData = new PropertyBag
			               {
			               	{"totalCount", hits.Total}
			               };

			// set the paging options if any
			if (query.From.HasValue && query.Size.HasValue)
			{
				metaData.Set("pageNumber", (query.From.Value + query.Size.Value)/query.Size.Value);
				metaData.Set("pageSize", query.Size.Value);
			}

			// set the sort value, if any
			var sortString = query.Sorts.Aggregate(",", (current, sort) =>
			                                            {
			                                            	var s = sort.ToString();
			                                            	if (String.IsNullOrEmpty(s))
			                                            		return current;
			                                            	return current + ',' + sort;
			                                            }).Trim(',', ' ');
			if (!String.IsNullOrEmpty(sortString))
				metaData.Set("sort", sortString);

			// return the meta data
			return metaData;
		}
		/// <summary>
		/// Maps all the <paramref name="hits"/> into <see cref="Record"/>s.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="hits">The <see cref="Hit"/>s.</param>
		/// <returns>Returns the mapped <see cref="Record"/>s.</returns>
		private static IEnumerable<Record> MapRecords(IMansionContext context, SearchQuery query, IEnumerable<Hit> hits)
		{
			// loop over all the hits
			foreach (var hit in hits)
			{
				// create the record
				var record = new Record();

				// find the type mapping
				var mapping = query.IndexDefinition.FindTypeMapping(hit.Type);

				// map all its properties
				mapping.MapProperties(context, hit, record);

				// initialize the record
				record.Initialize(context);

				// return the mapped record
				yield return record;
			}
		}
		/// <summary>
		/// Maps the properties from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The <see cref="Hit"/>.</param>
		/// <param name="target">The <see cref="IPropertyBag"/>.</param>
		private void MapProperties(IMansionContext context, Hit source, IPropertyBag target)
		{
			// loop over all the properties
			var document = JObject.Parse(source.Source);
			foreach (var property in document.Properties())
			{
				// find the property mapping for this property
				PropertyMapping propertyMapping;
				if (!Properties.TryGetValue(property.Name, out propertyMapping))
				{
					// just write the property without mapping
					SingleValuedPropertyMapping.Map(property, target);
					continue;
				}

				// map the value using the property mapping
				propertyMapping.Map(context, source, property, target);
			}
		}
		/// <summary>
		/// Maps the given <paramref name="response"/> into a <see cref="Nodeset"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="response">The <see cref="SearchResponse"/>.</param>
		/// <returns>Returns the mapped record set.</returns>
		public static Nodeset MapNodeset(IMansionContext context, SearchQuery query, SearchResponse response)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			if (response == null)
				throw new ArgumentNullException("response");

			// map all the hits
			var nodes = MapNodes(context, query, response.Hits.Hits);

			// map the set metadata
			var metaData = MapRecordSetMetaData(query, response.Hits);

			// create and return the set
			return new Nodeset(context, metaData, nodes);
		}
		/// <summary>
		/// Maps all the <paramref name="hits"/> into <see cref="Record"/>s.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="SearchQuery"/>.</param>
		/// <param name="hits">The <see cref="Hit"/>s.</param>
		/// <returns>Returns the mapped <see cref="Record"/>s.</returns>
		private static IEnumerable<Node> MapNodes(IMansionContext context, SearchQuery query, IEnumerable<Hit> hits)
		{
			// loop over all the hits
			foreach (var hit in hits)
			{
				// create the record
				var node = new Node();

				// find the type mapping
				var mapping = query.IndexDefinition.FindTypeMapping(hit.Type);

				// map all its properties
				mapping.MapProperties(context, hit, node);

				// initialize the record
				node.Initialize(context);

				// return the mapped record
				yield return node;
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this type.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Getsthe <see cref="PropertyMapping"/>s of this mapping.
		/// </summary>
		public IDictionary<string, PropertyMapping> Properties
		{
			get { return propertyMappings; }
		}
		#endregion
		#region Mapping Properties
		/// <summary>
		/// The <see cref="TypeMappingSource"/>.
		/// </summary>
		[JsonProperty("_source")]
		private TypeMappingSource Source { get; set; }
		/// <summary>
		/// Getsthe <see cref="PropertyMapping"/>s of this mapping.
		/// </summary>
		[JsonProperty("properties")]
		private IDictionary<string, PropertyMapping> MappedProperties
		{
			get { return propertyMappings.Where(candidate => !(candidate.Value is IgnoredPropertyMapping)).ToDictionary(x => x.Key, x => x.Value); }
		}
		#endregion
		#region Private Fields
		private readonly Dictionary<string, PropertyMapping> propertyMappings = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}