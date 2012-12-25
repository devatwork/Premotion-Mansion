using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// The unique token filter can be used to only index unique tokens during analysis. By default it is applied on all the token stream.
	/// </summary>
	public class UniqueTokenFilter : BaseTokenFilter
	{
		#region Nested type: UniqueTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="UniqueTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "uniqueTokenFilter")]
		private class UniqueTokenFilterDescriptor : BaseTokenFilterDescriptor
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
				var filter = new UniqueTokenFilter();

				bool onlyOnSamePosition;
				if (Properties.TryGet(context, "onlyOnSamePosition", out onlyOnSamePosition))
					filter.OnlyOnSamePosition = onlyOnSamePosition;

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
		public UniqueTokenFilter() : base("unique")
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// If only_on_same_position is set to true, it will only remove duplicate tokens on the same position.
		/// </summary>
		[JsonProperty("only_on_same_position ")]
		public bool OnlyOnSamePosition { get; set; }
		#endregion
	}
}