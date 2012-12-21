using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the base <see cref="TypeDescriptor"/> for elastic search properties.
	/// </summary>
	public abstract class SinglePropertyMappingBaseDescriptor : PropertyMappingBaseDescriptor
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
			// create the mapping
			var mapping = DoCreateSingleMapping(context, property);

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

			// map index
			string index;
			if (Properties.TryGet(context, "index", out index))
				mapping.Index = index;

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

			// add the property mapping to type mapping
			typeMapping.Add(mapping);
		}
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		protected abstract SinglePropertyMapping DoCreateSingleMapping(IMansionContext context, IPropertyDefinition property);
		#endregion
	}
}