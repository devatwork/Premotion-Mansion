using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Analysis
{
	/// <summary>
	/// A char filter of type html_strip stripping out HTML elements from an analyzed text.
	/// </summary>
	public class HtmlCharFilter : BaseCharFilter
	{
		#region Nested type: HtmlCharFilterDescriptor
		/// <summary>
		/// Descriptor for <see cref="HtmlCharFilter"/>.
		/// </summary>
		[TypeDescriptor(Constants.DescriptorNamespaceUri, "htmlCharacterFilter")]
		private class HtmlCharFilterDescriptor : BaseCharFilterDescriptor
		{
			#region Overrides of BaseCharFilterDescriptor
			/// <summary>
			/// Constructs a <see cref="BaseCharFilter"/> from this descriptor.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the created <see cref="BaseCharFilter"/>.</returns>
			protected override BaseCharFilter DoCreate(IMansionContext context)
			{
				return new HtmlCharFilter();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs the HTML char filter.
		/// </summary>
		public HtmlCharFilter() : base("html_strip")
		{
		}
		#endregion
	}
}