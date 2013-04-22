using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// Describes a many-to-many relation ship from a multiple sources to multiple targets.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "manyToMany")]
	public class ManyToManyLinkDescriptor : LinkDescriptor
	{
		/// <summary>
		/// Gets the <see cref="LinkDefinition"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="name">The name of the link definition.</param>
		/// <returns>Returns the <see cref="LinkDefinition"/>.</returns>
		protected override LinkDefinition GetDefinition(IMansionContext context, string name)
		{
			return new ManyToManyLinkDefinition(name);
		}
	}
}