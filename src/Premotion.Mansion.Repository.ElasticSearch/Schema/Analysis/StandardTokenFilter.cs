using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A token filter of type standard that normalizes tokens extracted with the Standard Tokenizer.
	/// </summary>
	public class StandardTokenFilter : BaseTokenFilter
	{
		#region Nested type: StandardTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="StandardTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "standardTokenFilter")]
		private class StandardTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Overrides of BaseTokenFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenFilter"/>.</returns>
			protected override BaseTokenFilter DoCreate(IMansionContext context)
			{
				return new StandardTokenFilter();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a standard token filter
		/// </summary>
		public StandardTokenFilter() : base("standard")
		{
		}
		#endregion
	}
}