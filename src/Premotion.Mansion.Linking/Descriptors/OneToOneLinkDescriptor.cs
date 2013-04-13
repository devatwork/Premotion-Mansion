using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a one-to-one relation ship from a single source to a single single target.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "oneToOne")]
	public class OneToOneLinkDescriptor : BidirectionalLinkDescriptor
	{
	}
}