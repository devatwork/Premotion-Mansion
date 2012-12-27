using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// An analyzer of type custom that allows to combine a Tokenizer with zero or more Token Filters, and zero or more Char Filters. The custom analyzer accepts a logical/registered name of the tokenizer to use, and a list of logical/registered names of token filters.
	/// </summary>
	public class CustomAnalyzer : BaseAnalyzer
	{
		#region Nested type: CustomAnalyzerDescriptor
		/// <summary>
		/// Descriptor for <see cref="CustomAnalyzer"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "customAnalyzer")]
		private class CustomAnalyzerDescriptor : BaseAnalyzerDescriptor
		{
			#region Overrides of BaseAnalysisDescriptor<BaseAnalyzer>
			/// <summary>
			/// Creates the <see cref="BaseAnalyzer"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseAnalyzer"/>.</returns>
			protected override BaseAnalyzer DoCreate(IMansionContext context)
			{
				// get the tokenizer
				var tokenizer = Properties.Get<string>(context, "tokenizer");
				if (string.IsNullOrEmpty(tokenizer))
					throw new InvalidOperationException("Custom analyzers must have a tokenizer");

				// get the tokenFilters
				var tokenFilters = Properties.Get(context, "tokenFilters", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

				// get the charFilters
				var charFilters = Properties.Get(context, "charFilters", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

				// create the analyzer
				return new CustomAnalyzer(tokenizer, tokenFilters, charFilters);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a custom <see cref="BaseAnalyzer"/>.
		/// </summary>
		/// <param name="tokenizer">The <see cref="Tokenizer"/>.</param>
		/// <param name="tokenFilters">The <see cref="Filters"/>.</param>
		/// <param name="charFilters">The <see cref="CharFilters"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="tokenizer"/> is null or empty.</exception>
		public CustomAnalyzer(string tokenizer, IEnumerable<string> tokenFilters, IEnumerable<string> charFilters) : base("custom")
		{
			// validate arguments
			if (string.IsNullOrEmpty(tokenizer))
				throw new ArgumentNullException("tokenizer");

			// set the values
			Tokenizer = tokenizer.ToLower();
			Filters = tokenFilters.Select(x => x.ToLower()).ToArray();
			CharFilters = charFilters.Select(x => x.ToLower()).ToArray();
		}
		#endregion
		#region Properties
		/// <summary>
		/// The logical / registered name of the tokenizer to use.
		/// </summary>
		[JsonProperty("tokenizer")]
		public string Tokenizer { get; private set; }
		/// <summary>
		/// An optional list of logical / registered name of token filters.
		/// </summary>
		[JsonProperty("filter", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] Filters { get; private set; }
		/// <summary>
		/// An optional list of logical / registered name of char filters.
		/// </summary>
		[JsonProperty("char_filter", DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] CharFilters { get; private set; }
		#endregion
	}
}