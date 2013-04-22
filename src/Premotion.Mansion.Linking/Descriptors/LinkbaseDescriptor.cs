using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Linking.Descriptors
{
	/// <summary>
	/// This type has a linkbase, indicating that rich links can be made from and to this type.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "linkbase")]
	public class LinkbaseDescriptor : NestedTypeDescriptor
	{
		/// <summary>
		/// Gets the <see cref="LinkbaseDefinition"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public LinkbaseDefinition GetDefinition(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get all the child descriptors
			var linkDefinitions = GetDescriptors<LinkDescriptor>().Select(linkDescriptor => linkDescriptor.GetDefinition(context));

			// create the definition
			return new LinkbaseDefinition(TypeDefinition, linkDefinitions);
		}
	}
}