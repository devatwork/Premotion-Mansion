using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search properties.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "propertyMapping")]
	public class PropertyMappingDescriptor : TypeDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		public PropertyMapping CreateMapping(IMansionContext context, IPropertyDefinition property)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (property == null)
				throw new ArgumentNullException("property");

			// create the mapping
			var mapping = new PropertyMapping(property)
			              {
			              	// map the type
			              	Type = Properties.Get<string>(context, "type"),
			              	Index = Properties.Get<string>(context, "index"),
			              };

			// map nullValue
			object nullValue;
			if (Properties.TryGet(context, "nullValue", out nullValue))
				mapping.NullValue = nullValue;

			// map indexValue
			string indexName;
			if (Properties.TryGet(context, "indexName", out indexName))
				mapping.IndexName = indexName;

			// map indexValue
			string store;
			if (Properties.TryGet(context, "store", out store))
				mapping.Store = store;

			// map precisionStep
			int precisionStep;
			if (Properties.TryGet(context, "precisionStep", out precisionStep))
				mapping.PrecisionStep = precisionStep;

			// map boost
			double boost;
			if (Properties.TryGet(context, "boost", out boost))
				mapping.Boost = boost;

			// map includeInAll
			bool includeInAll;
			if (Properties.TryGet(context, "includeInAll", out includeInAll))
				mapping.IncludeInAll = includeInAll;

			// map termVector
			string termVector;
			if (Properties.TryGet(context, "termVector", out termVector))
				mapping.TermVector = termVector;

			// map omitNorms
			bool omitNorms;
			if (Properties.TryGet(context, "omitNorms", out omitNorms))
				mapping.OmitNorms = omitNorms;

			// map omitTermFreqAndPositions
			bool omitTermFreqAndPositions;
			if (Properties.TryGet(context, "omitTermFreqAndPositions", out omitTermFreqAndPositions))
				mapping.OmitTermFreqAndPositions = omitTermFreqAndPositions;

			// map analyzer
			string analyzer;
			if (Properties.TryGet(context, "analyzer", out analyzer))
				mapping.Analyzer = analyzer;

			// map indexAnalyzer
			string indexAnalyzer;
			if (Properties.TryGet(context, "indexAnalyzer", out indexAnalyzer))
				mapping.IndexAnalyzer = indexAnalyzer;

			// map indexAnalyzer
			string searchAnalyzer;
			if (Properties.TryGet(context, "searchAnalyzer", out searchAnalyzer))
				mapping.SearchAnalyzer = searchAnalyzer;

			// return the mapping
			return mapping;
		}
		#endregion
	}
}