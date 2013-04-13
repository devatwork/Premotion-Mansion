using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// This type has a linkbase, indicating that rich links can be made from and to this type.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "linkbase")]
	public class LinkbaseDescriptor : TypeDescriptor
	{
	}
}