using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A tokenizer of type keyword that emits the entire input as a single input.
	/// </summary>
	public class KeywordTokenizer : BaseTokenizer
	{
		#region Nested type: KeywordTokenizerDescriptor
		/// <summary>
		/// Descriptor for <see cref="KeywordTokenizer"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "keywordTokenizer")]
		private class KeywordTokenizerDescriptor : BaseTokenizerDescriptor
		{
			#region Overrides of BaseTokenizerDescriptor
			/// <summary>
			/// Creates the <see cref="BaseTokenizer"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenizer"/>.</returns>
			protected override BaseTokenizer DoCreate(IMansionContext context)
			{
				var filter = new KeywordTokenizer();

				int bufferSize;
				if (Properties.TryGet(context, "bufferSize", out bufferSize))
					filter.BufferSize = bufferSize;

				return filter;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this token filter
		/// </summary>
		public KeywordTokenizer() : base("keyword")
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// The term buffer size. Defaults to 256.
		/// </summary>
		[JsonProperty("buffer_size")]
		public int? BufferSize { get; set; }
		#endregion
	}
}