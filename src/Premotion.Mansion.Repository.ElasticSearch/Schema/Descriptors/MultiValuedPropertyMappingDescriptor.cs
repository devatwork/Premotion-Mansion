using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for multi-valued properties.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "multiValuedProperty")]
	public class MultiValuedPropertyMappingDescriptor : PropertyMappingBaseDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		protected override PropertyMapping DoCreateMapping(IMansionContext context, IPropertyDefinition property)
		{
			// create the mapping
			return new MultiValuedPropertyMapping(property)
			       {
			       	// map the type
			       	Type = Properties.Get<string>(context, "type")
			       };
		}
		#endregion
	}
}