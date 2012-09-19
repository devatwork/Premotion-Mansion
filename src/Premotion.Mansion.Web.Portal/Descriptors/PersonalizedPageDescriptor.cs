using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Marks a page as personalized. The page block will then render the content of the page is the second render pass.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "personalizedPage")]
	public class PersonalizedPageDescriptor : TypeDescriptor
	{
	}
}