using System;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// Overrides stemming algorithms, by applying a custom mapping, then protecting these terms from being modified by stemmers. Must be placed before any stemming filters.
	/// </summary>
	public class StemmerOverrideTokenFilter : BaseTokenFilter
	{
		#region Nested type: StemmerOverrideTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="StemmerOverrideTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "stemmerOverrideTokenFilter")]
		private class StemmerOverrideTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="expressionScriptService"></param>
			public StemmerOverrideTokenFilterDescriptor(IExpressionScriptService expressionScriptService)
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
				var filter = new StemmerOverrideTokenFilter();

				bool ignoreCase;
				if (Properties.TryGet(context, "ignoreCase", out ignoreCase))
					filter.IgnoreCase = ignoreCase;

				string rulesPath;
				if (Properties.TryGet(context, "rulesPath", out rulesPath))
					filter.RulesPath = rulesPath;

				string rules;
				if (Properties.TryGet(context, "rules", out rules))
				{
					// execute any expressions
					filter.Rules = expressionScriptService.Parse(context, new LiteralResource(rules)).Execute<string>(context).Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
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
		/// Constructs the <see cref="StemmerOverrideTokenFilter"/>.
		/// </summary>
		public StemmerOverrideTokenFilter() : base("stemmer_override")
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
		/// A path (either relative to config location, or absolute) to a list of rules.
		/// </summary>
		[JsonProperty("rules_path", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string RulesPath { get; set; }
		/// <summary>
		/// A list of rules to use.
		/// </summary>
		[JsonProperty("rules", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] Rules { get; set; }
		#endregion
	}
}