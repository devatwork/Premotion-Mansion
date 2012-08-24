using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a <see cref="NodePointerPropertyColumn"/>.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "treeRelations")]
	public class NodePointerColumnDescriptor : ColumnDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the a <see cref="Column"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="table">The <see cref="Table"/> to which the column will be added.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Column"/>.</returns>
		protected override Column DoCreate(IMansionContext context, Table table, IPropertyDefinition property)
		{
			// create the column
			return new NodePointerPropertyColumn(table, property.Name);
		}
		#endregion
	}
}