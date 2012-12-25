using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A tokenizer of type standard providing grammar based tokenizer that is a good tokenizer for most European language documents. The tokenizer implements the Unicode Text Segmentation algorithm, as specified in Unicode Standard Annex #29.
	/// </summary>
	public class StandardTokenizer : BaseTokenizer
	{
		#region Nested type: StandardTokenizerDescriptor
		/// <summary>
		/// Descriptor for <see cref="StandardTokenizer"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "standardTokenizer")]
		private class StandardTokenizerDescriptor : BaseTokenizerDescriptor
		{
			#region Overrides of BaseTokenizerDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenizer"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenizer"/>.</returns>
			protected override BaseTokenizer DoCreate(IMansionContext context)
			{
				// create the tokenizer
				var tokenizer = new StandardTokenizer();

				int maxTokenLength;
				if (Properties.TryGet(context, "maxTokenLength", out maxTokenLength))
					tokenizer.MaxTokenLength = maxTokenLength;

				// return the confifured tokenizer
				return tokenizer;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this tokenizer.
		/// </summary>
		public StandardTokenizer() : base("standard")
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// The maximum token length. If a token is seen that exceeds this length then it is discarded. Defaults to 255.
		/// </summary>
		[JsonProperty("max_token_length", NullValueHandling = NullValueHandling.Ignore)]
		public int? MaxTokenLength { get; set; }
		#endregion
	}
}