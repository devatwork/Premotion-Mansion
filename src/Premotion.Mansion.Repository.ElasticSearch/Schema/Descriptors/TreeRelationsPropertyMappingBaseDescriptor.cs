using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors
{
	/// <summary>
	/// Represents the <see cref="TypeDescriptor"/> for elastic search tree relation properties.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "treeRelations")]
	public class TreeRelationsPropertyMappingBaseDescriptor : PropertyMappingBaseDescriptor
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
			typeMapping.Add(new TreeRelationsPropertyMapping());
			typeMapping.Add(new SingleValuedPropertyMapping("name")
			                {
			                	Type = "string",
			                	Index = "analyzed"
			                });
			typeMapping.Add(new SingleValuedPropertyMapping("type")
			                {
			                	Type = "string",
			                	Index = "not_analyzed"
			                });
			typeMapping.Add(new IgnoredPropertyMapping("depth"));
			typeMapping.Add(new IgnoredPropertyMapping("parentId"));
			typeMapping.Add(new IgnoredPropertyMapping("parentPointer"));
			typeMapping.Add(new IgnoredPropertyMapping("parentPath"));
			typeMapping.Add(new IgnoredPropertyMapping("parentStructure"));
		}
		#endregion
	}
}