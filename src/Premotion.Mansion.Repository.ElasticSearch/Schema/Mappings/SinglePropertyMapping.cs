using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
{
	/// <summary>
	/// Maps a single property.
	/// </summary>
	[JsonConverter(typeof (SinglePropertyMappingConverter))]
	public abstract class SinglePropertyMapping : PropertyMapping
	{
		#region Nested type: SinglePropertyMappingConverter
		/// <summary>
		/// Maps <see cref="SinglePropertyMapping"/>.
		/// </summary>
		private class SinglePropertyMappingConverter : BaseWriteConverter<SinglePropertyMapping>
		{
			#region Overrides of BaseWriteConverter<SinglePropertyMapping>
			/// <summary>
			/// Writes the JSON representation of the object.
			/// </summary>
			/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
			protected override void DoWriteJson(JsonWriter writer, SinglePropertyMapping value, JsonSerializer serializer)
			{
				// write type
				writer.WritePropertyName("type");
				writer.WriteValue(value.Type);

				// write null_value
				if (value.NullValue != null)
				{
					writer.WritePropertyName("null_value");
					writer.WriteValue(value.NullValue);
				}

				// write index_name
				if (!string.IsNullOrEmpty(value.IndexName))
				{
					writer.WritePropertyName("index_name");
					writer.WriteValue(value.IndexName);
				}

				// write store
				if (!string.IsNullOrEmpty(value.Store))
				{
					writer.WritePropertyName("store");
					writer.WriteValue(value.Store);
				}

				// write precision_step
				if (value.PrecisionStep.HasValue)
				{
					writer.WritePropertyName("precision_step");
					writer.WriteValue(value.PrecisionStep.Value);
				}

				// write index
				if (!string.IsNullOrEmpty(value.Index))
				{
					writer.WritePropertyName("index");
					writer.WriteValue(value.Index);
				}

				// write boost
				if (value.Boost.HasValue)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(value.Boost.Value);
				}

				// write include_in_all
				if (value.IncludeInAll.HasValue)
				{
					writer.WritePropertyName("include_in_all");
					writer.WriteValue(value.IncludeInAll.Value);
				}

				// write format
				if (!string.IsNullOrEmpty(value.DateFormat))
				{
					writer.WritePropertyName("format");
					writer.WriteValue(value.DateFormat);
				}

				// write term_vector
				if (!string.IsNullOrEmpty(value.TermVector))
				{
					writer.WritePropertyName("term_vector");
					writer.WriteValue(value.TermVector);
				}

				// write omit_norms
				if (value.OmitNorms.HasValue)
				{
					writer.WritePropertyName("omit_norms");
					writer.WriteValue(value.OmitNorms.Value);
				}

				// write omit_term_freq_and_positions
				if (value.OmitTermFreqAndPositions.HasValue)
				{
					writer.WritePropertyName("omit_term_freq_and_positions");
					writer.WriteValue(value.OmitTermFreqAndPositions.Value);
				}

				// write analyzer
				if (!string.IsNullOrEmpty(value.Analyzer))
				{
					writer.WritePropertyName("analyzer");
					writer.WriteValue(value.Analyzer);
				}

				// write index_analyzer
				if (!string.IsNullOrEmpty(value.IndexAnalyzer))
				{
					writer.WritePropertyName("index_analyzer");
					writer.WriteValue(value.IndexAnalyzer);
				}

				// write search_analyzer
				if (!string.IsNullOrEmpty(value.SearchAnalyzer))
				{
					writer.WritePropertyName("search_analyzer");
					writer.WriteValue(value.SearchAnalyzer);
				}
			}
			#endregion
		}
		#endregion
		#region Nested type: SinglePropertyMappingDescriptor
		/// <summary>
		/// Represents the base <see cref="TypeDescriptor"/> for elastic search properties.
		/// </summary>
		public abstract class SinglePropertyMappingDescriptor : PropertyMappingDescriptor
		{
			#region Constructors
			/// <summary>
			/// Constructs a <see cref="SinglePropertyMappingDescriptor"/>.
			/// </summary>
			/// <param name="expressionScriptService">The <see cref="IExpressionScriptService"/></param>
			protected SinglePropertyMappingDescriptor(IExpressionScriptService expressionScriptService)
			{
				// validate arguments
				if (expressionScriptService == null)
					throw new ArgumentNullException("expressionScriptService");

				// set the value
				this.expressionScriptService = expressionScriptService;
			}
			#endregion
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

				// map normalize method
				string normalizeExpression;
				if (Properties.TryGet(context, "normalizeExpression", out normalizeExpression))
					mapping.NormalizeExpression = expressionScriptService.Parse(context, new LiteralResource(normalizeExpression));

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
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructor
		/// <summary>
		/// Constructs the property mapping with the given <paramref name="field"/>.
		/// </summary>
		/// <param name="field">The name of the property mapped by this mapper.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		protected SinglePropertyMapping(string field) : base(field)
		{
			// set values
			Type = "string";
			Index = "analyzed";
		}
		#endregion
		#region Overrides of PropertyMapping
		/// <summary>
		/// Gets a flag indicating whether this field is analyzed or not.
		/// </summary>
		public override bool IsAnalyzed
		{
			get { return "analyzed".Equals(Index, StringComparison.OrdinalIgnoreCase); }
		}
		/// <summary>
		/// Normalizes the given <paramref name="value"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="value">The value which to normalize.</param>
		/// <returns>Returns the normalized values.</returns>
		protected override object DoNormalize(IMansionContext context, object value)
		{
			// check for not_analyzed string properties
			if ("not_analyzed".Equals(Index, StringComparison.OrdinalIgnoreCase) && "string".Equals(Type, StringComparison.OrdinalIgnoreCase))
				return (value as string ?? string.Empty).ToLower();

			// check if there is no normalize expression
			if (NormalizeExpression == null)
				return value;

			using (context.Stack.Push("Row", new PropertyBag {
				{"value", value}
			}))
				return NormalizeExpression.Execute<object>(context);
		}
		#endregion
		#region Properties
		/// <summary>
		/// The type of the number. Can be string, float, double, integer, long, short, byte, date, boolean, binary. Required.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }
		/// <summary>
		/// When there is a (JSON) null value for the field, use the null_value as the field value. Defaults to not adding the field at all.
		/// </summary>
		[JsonProperty("null_value")]
		public object NullValue { get; set; }
		/// <summary>
		/// The name of the field that will be stored in the index. Defaults to the property/field name.
		/// </summary>
		[JsonProperty("index_name")]
		public string IndexName { get; set; }
		/// <summary>
		/// Set to yes to store actual field in the index, no to not store it. Defaults to no (note, the JSON document itself is stored, and it can be retrieved from it).
		/// </summary>
		[JsonProperty("store")]
		public string Store { get; set; }
		/// <summary>
		/// Set to analyzed for the field to be indexed and searchable after being broken down into token using an analyzer. not_analyzed means that its still searchable, but does not go through any analysis process or broken down into tokens. no means that it won’t be searchable at all (as an individual field; it may still be included in _all). Defaults to analyzed.
		/// </summary>
		[JsonProperty("index")]
		public string Index { get; set; }
		/// <summary>
		/// The precision step (number of terms generated for each number value). Defaults to 4.
		/// </summary>
		[JsonProperty("precision_step")]
		public int? PrecisionStep { get; set; }
		/// <summary>
		/// The boost value. Defaults to 1.0.
		/// </summary>
		[JsonProperty("boost")]
		public double? Boost { get; set; }
		/// <summary>
		/// Should the field be included in the _all field (if enabled). Defaults to true or to the parent object type setting.
		/// </summary>
		[JsonProperty("include_in_all")]
		public bool? IncludeInAll { get; set; }
		/// <summary>
		/// The date format. Defaults to dateOptionalTime.
		/// </summary>
		[JsonProperty("format")]
		public string DateFormat { get; set; }
		/// <summary>
		/// Possible values are no, yes, with_offsets, with_positions, with_positions_offsets. Defaults to no.
		/// </summary>
		[JsonProperty("term_vector")]
		public string TermVector { get; set; }
		/// <summary>
		/// Boolean value if norms should be omitted or not. Defaults to false.
		/// </summary>
		[JsonProperty("omit_norms")]
		public bool? OmitNorms { get; set; }
		/// <summary>
		/// Boolean value if term freq and positions should be omitted. Defaults to false. Deprecated since 0.20, see index_options.
		/// </summary>
		[JsonProperty("omit_term_freq_and_positions")]
		public bool? OmitTermFreqAndPositions { get; set; }
		/// <summary>
		/// The analyzer used to analyze the text contents when analyzed during indexing and when searching using a query string. Defaults to the globally configured analyzer.
		/// </summary>
		[JsonProperty("analyzer")]
		public string Analyzer { get; set; }
		/// <summary>
		/// The analyzer used to analyze the text contents when analyzed during indexing.
		/// </summary>
		[JsonProperty("index_analyzer")]
		public string IndexAnalyzer { get; set; }
		/// <summary>
		/// The analyzer used to analyze the field when part of a query string.
		/// </summary>
		[JsonProperty("search_analyzer")]
		public string SearchAnalyzer { get; set; }
		/// <summary>
		/// Gets the normalize <see cref="IScript"/> expression.
		/// </summary>
		[JsonIgnore]
		private IScript NormalizeExpression { get; set; }
		#endregion
	}
}