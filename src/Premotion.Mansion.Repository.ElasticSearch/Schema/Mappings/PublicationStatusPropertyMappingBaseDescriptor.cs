using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Mappings
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search publication status properties.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "publicationStatus")]
	public class PublicationStatusPropertyMappingBaseDescriptor : PropertyMappingBaseDescriptor
	{
		#region Overrides of PropertyMappingBaseDescriptor
		/// <summary>
		/// Adds <see cref="PropertyMapping"/>s of <paramref name="property"/> to the given <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/> of the property for which to add the <see cref="PropertyMapping"/>s.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/> to which to add the new <see cref="PropertyMapping"/>s.</param>
		protected override void DoAddMappingTo(IMansionContext context, IPropertyDefinition property, TypeMapping typeMapping)
		{
			typeMapping.Add(new SingleValuedPropertyMapping("approved")
			                {
			                	Type = "boolean"
			                });
			typeMapping.Add(new SingleValuedPropertyMapping("archived")
			                {
			                	Type = "boolean"
			                });
			typeMapping.Add(new SingleValuedPropertyMapping("publicationDate")
			                {
			                	Type = "date"
			                });
			typeMapping.Add(new SingleValuedPropertyMapping("expirationDate")
			                {
			                	Type = "date"
			                });
		}
		#endregion
	}
}