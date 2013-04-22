using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a one-to-many relation ship from a single source to multiple targets.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "oneToMany")]
	public class OneToManyLinkDescriptor : LinkDescriptor
	{
		/// <summary>
		/// Gets the <see cref="LinkDefinition"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="name">The name of the link definition.</param>
		/// <returns>Returns the <see cref="LinkDefinition"/>.</returns>
		protected override LinkDefinition GetDefinition(IMansionContext context, string name)
		{
			return new OneToManyLinkDefinition(name);
		}
	}
}