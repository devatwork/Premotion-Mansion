using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a many-to-many relation ship from a multiple sources to multiple targets.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "manyToMany")]
	public class ManyToManyLinkDescriptor : BidirectionalLinkDescriptor
	{
	}
}