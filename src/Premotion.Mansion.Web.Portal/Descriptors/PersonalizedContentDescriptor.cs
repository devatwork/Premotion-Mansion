using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Marks content as personalized. The content detail block will then render the content in the second render pass.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "personalizedContent")]
	public class PersonalizedContentDescriptor : TypeDescriptor
	{
	}
}