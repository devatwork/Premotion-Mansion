using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core.Types;

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
		protected PropertyMapping(IPropertyDefinition property)
		{
			// validate arguments
			if (property == null)
				throw new ArgumentNullException("property");

			// set value
			Name = property.Name.ToLower();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		[JsonIgnore]
		public string Name { get; private set; }
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
		#endregion
	}
}