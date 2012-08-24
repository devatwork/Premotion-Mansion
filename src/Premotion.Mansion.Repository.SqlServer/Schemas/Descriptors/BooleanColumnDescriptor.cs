using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a column.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "booleanColumn")]
	public class BooleanColumnDescriptor : ColumnDescriptor
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
			// get the column name
			var columnName = Properties.Get<string>(context, "columnName", null) ?? property.Name;

			// create the column
			return BooleanPropertyColumn.CreateBooleanColumn(context, property.Name, columnName, Properties);
		}
		#endregion
	}
}