using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Templating.Html
{
	/// <summary>
	/// Defines the descriptor for template headers.
	/// </summary>
	[Named("http://schemas.premotion.nl/mansion/1.0/template.xsd", "section")]
	public class HtmlSectionHeaderDescriptor : Descriptor
	{
		#region Constructors
		/// <summary>
		/// Constructs a descriptor.
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		public HtmlSectionHeaderDescriptor(string namespaceUri, string name, IPropertyBag properties) : base(namespaceUri, name, properties)
		{
		}
		#endregion
	}
}