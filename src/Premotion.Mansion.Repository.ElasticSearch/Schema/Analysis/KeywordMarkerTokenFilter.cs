using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Protects words from being modified by stemmers.
	/// </summary>
	public class KeywordMarkerTokenFilter : BaseTokenFilter
	{
		#region Nested type: KeywordMarkerTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="KeywordMarkerTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "keywordMarkerTokenFilter")]
		private class KeywordMarkerTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="expressionScriptService"></param>
			public KeywordMarkerTokenFilterDescriptor(IExpressionScriptService expressionScriptService)
			{
				// validate arguments
				if (expressionScriptService == null)
					throw new ArgumentNullException("expressionScriptService");

				// set the values
				this.expressionScriptService = expressionScriptService;
			}
			#endregion
			#region Overrides of BaseTokenFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenFilter"/>.</returns>
			protected override BaseTokenFilter DoCreate(IMansionContext context)
			{
				// create the filter
				var filter = new KeywordMarkerTokenFilter();

				bool ignoreCase;
				if (Properties.TryGet(context, "ignoreCase", out ignoreCase))
					filter.IgnoreCase = ignoreCase;

				string keywordsPath;
				if (Properties.TryGet(context, "keywordsPath", out keywordsPath))
					filter.KeywordsPath = keywordsPath;

				string keywords;
				if (Properties.TryGet(context, "keywords", out keywords))
				{
					// execute any expressions
					filter.Keywords = expressionScriptService.Parse(context, new LiteralResource(keywords)).Execute<string>(context).Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
				}

				// return the configured filter
				return filter;
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScriptService expressionScriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the <see cref="KeywordMarkerTokenFilter"/>.
		/// </summary>
		public KeywordMarkerTokenFilter() : base("keyword_marker")
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Set to true to lower case all words first. Defaults to false.
		/// </summary>
		[JsonProperty("ignore_case")]
		public bool IgnoreCase { get; set; }
		/// <summary>
		/// A path (either relative to config location, or absolute) to a list of words.
		/// </summary>
		[JsonProperty("keywords_path", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string KeywordsPath { get; set; }
		/// <summary>
		/// A list of words to use.
		/// </summary>
		[JsonProperty("keywords", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] Keywords { get; set; }
		#endregion
	}
}