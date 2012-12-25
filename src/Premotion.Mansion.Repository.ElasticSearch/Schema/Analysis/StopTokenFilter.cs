using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A token filter of type stop that removes stop words from token streams.
	/// </summary>
	public class StopTokenFilter : BaseTokenFilter
	{
		#region Nested type: StopTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="StopTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "stopTokenFilter")]
		private class StopTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Overrides of BaseTokenFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenFilter"/>.</returns>
			protected override BaseTokenFilter DoCreate(IMansionContext context)
			{
				// create the filter
				var filter = new StopTokenFilter();

				bool enablePositionIncrements;
				if (Properties.TryGet(context, "enablePositionIncrements", out enablePositionIncrements))
					filter.EnablePositionIncrements = enablePositionIncrements;

				bool ignoreCase;
				if (Properties.TryGet(context, "ignoreCase", out ignoreCase))
					filter.IgnoreCase = ignoreCase;

				string stopwordsPath;
				if (Properties.TryGet(context, "stopwordsPath", out stopwordsPath))
					filter.StopwordsPath = stopwordsPath;

				string stopwords;
				if (Properties.TryGet(context, "stopwords", out stopwords))
					filter.Stopwords = stopwords;

				// return the configured filter
				return filter;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="StopTokenFilter"/>.
		/// </summary>
		public StopTokenFilter() : base("stop")
		{
			EnablePositionIncrements = true;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Set to true if token positions should record the removed stop words, false otherwise. Defaults to true.
		/// </summary>
		[JsonProperty("enable_position_increments")]
		public bool EnablePositionIncrements { get; set; }
		/// <summary>
		/// Set to true to lower case all words first. Defaults to false.
		/// </summary>
		[JsonProperty("ignore_case")]
		public bool IgnoreCase { get; set; }
		/// <summary>
		/// A path (either relative to config location, or absolute) to a stopwords file configuration.
		/// </summary>
		[JsonProperty("stopwords_path")]
		public string StopwordsPath { get; set; }
		/// <summary>
		/// A list of stop words to use. Defaults to english stop words.
		/// </summary>
		[JsonProperty("stopwords")]
		public string Stopwords { get; set; }
		#endregion
	}
}