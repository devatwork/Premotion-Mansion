using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a one-to-many relation ship from a single source to multiple targets.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "oneToMany")]
	public class OneToManyLinkDescriptor : BidirectionalLinkDescriptor
	{
	}
}