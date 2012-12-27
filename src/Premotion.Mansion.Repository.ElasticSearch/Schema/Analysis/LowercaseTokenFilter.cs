using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A token filter of type lowercase that normalizes token text to lower case.
	/// </summary>
	public class LowercaseTokenFilter : BaseTokenFilter
	{
		#region Nested type: LowercaseTokenFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="LowercaseTokenFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "lowercaseTokenFilter")]
		private class LowercaseTokenFilterDescriptor : BaseTokenFilterDescriptor
		{
			#region Overrides of BaseTokenFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseTokenFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseTokenFilter"/>.</returns>
			protected override BaseTokenFilter DoCreate(IMansionContext context)
			{
				return new LowercaseTokenFilter();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this token filter
		/// </summary>
		public LowercaseTokenFilter() : base("lowercase")
		{
		}
		#endregion
	}
}