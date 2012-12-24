using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search properties.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "singleValuedProperty")]
	public class SingleValuedPropertyMappingDescriptor : SinglePropertyMappingBaseDescriptor
	{
		#region Create Methods
		/// <summary>
		/// Creates a <see cref="PropertyMapping"/> from this descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>The created <see cref="PropertyMapping"/>.</returns>
		protected override SinglePropertyMapping DoCreateSingleMapping(IMansionContext context, IPropertyDefinition property)
		{
			// create the mapping
			return new SingleValuedPropertyMapping(property.Name)
			       {
			       	// map the type
			       	Type = Properties.Get<string>(context, "type")
			       };
		}
		#endregion
	}
}