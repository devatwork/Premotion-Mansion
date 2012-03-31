using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Templating.Html
{
	/// <summary>
	/// Defines the descriptor for template headers.
	/// </summary>
	[Named(typeof (IDescriptor), "http://schemas.premotion.nl/mansion/1.0/template.xsd", "section")]
	public class HtmlSectionHeaderDescriptor : Descriptor
	{
	}
}