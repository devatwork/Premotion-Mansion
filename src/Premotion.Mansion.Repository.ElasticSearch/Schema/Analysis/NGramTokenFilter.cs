using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A token filter of type nGram.
	/// </summary>
	public class NGramTokenFilter : BaseTokenFilter
	{
		#region Nested type: NGramTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="NGramTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "nGramTokenFilter")]
		private class NGramTokenFilterDescriptor : BaseTokenFilterDescriptor
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
				var filter = new NGramTokenFilter();

				int minGram;
				if (Properties.TryGet(context, "minGram", out minGram))
					filter.MinGram = minGram;

				int maxGram;
				if (Properties.TryGet(context, "maxGram", out maxGram))
					filter.MaxGram = maxGram;

				// return the filter
				return filter;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this token filter
		/// </summary>
		public NGramTokenFilter() : base("nGram")
		{
			MinGram = 1;
			MaxGram = 2;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Defaults to 1.
		/// </summary>
		[JsonProperty("min_gram")]
		public int MinGram { get; set; }
		/// <summary>
		/// Defaults to 2.
		/// </summary>
		[JsonProperty("max_gram")]
		public int MaxGram { get; set; }
		#endregion
	}
}