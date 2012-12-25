using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// The synonym token filter allows to easily handle synonyms during the analysis process. 
	/// </summary>
	public class SynonymTokenFilter : BaseTokenFilter
	{
		#region Nested type: SynonymTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="SynonymTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "synonymTokenFilter")]
		private class SynonymTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="expressionScriptService"></param>
			public SynonymTokenFilterDescriptor(IExpressionScriptService expressionScriptService)
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
				var filter = new SynonymTokenFilter();

				string synonymsPath;
				if (Properties.TryGet(context, "synonymsPath", out synonymsPath))
					filter.SynonymsPath = synonymsPath;

				string synonyms;
				if (Properties.TryGet(context, "synonyms", out synonyms))
					filter.Synonyms = expressionScriptService.Parse(context, new LiteralResource(synonyms)).Execute<string>(context).Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

				bool ignoreCase;
				if (Properties.TryGet(context, "ignoreCase", out ignoreCase))
					filter.IgnoreCase = ignoreCase;

				bool expand;
				if (Properties.TryGet(context, "expand", out expand))
					filter.Expand = expand;

				// return the filter
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
		/// Constructs this token filter
		/// </summary>
		public SynonymTokenFilter() : base("synonym")
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Set to true if token positions should record the inserted tokens, defaults to true.
		/// </summary>
		[JsonProperty("expand", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Expand { get; set; }
		/// <summary>
		/// Set to true to lower case all words first. Defaults to false.
		/// </summary>
		[JsonProperty("ignore_case")]
		public bool IgnoreCase { get; set; }
		/// <summary>
		/// A path (either relative to config location, or absolute) to a synonyms file configuration.
		/// </summary>
		[JsonProperty("synonyms_path", NullValueHandling = NullValueHandling.Ignore)]
		public string SynonymsPath { get; set; }
		/// <summary>
		/// A list of synonyms to use.
		/// </summary>
		[JsonProperty("synonyms", NullValueHandling = NullValueHandling.Ignore)]
		public string[] Synonyms { get; set; }
		#endregion
	}
}