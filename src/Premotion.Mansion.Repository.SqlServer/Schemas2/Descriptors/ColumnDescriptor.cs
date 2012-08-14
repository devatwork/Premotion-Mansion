using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2.Descriptors
{
	/// <summary>
	/// Describes a column.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "column")]
	public class ColumnDescriptor : TypeDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the a <see cref="Column"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="table">The <see cref="Table"/> to which the column will be added.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Column"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public Column Create(IMansionContext context, Table table, IPropertyDefinition property)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (property == null)
				throw new ArgumentNullException("property");
			if (table == null)
				throw new ArgumentNullException("table");

			// invoke template method
			return DoCreate(context, table, property);
		}
		/// <summary>
		/// Creates the a <see cref="Column"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="table">The <see cref="Table"/> to which the column will be added.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Column"/>.</returns>
		protected virtual Column DoCreate(IMansionContext context, Table table, IPropertyDefinition property)
		{
			// get the column name
			var columnName = Properties.Get<string>(context, "columnName", null) ?? property.Name;

			// create the column
			return PropertyColumn.CreatePropertyColumn(context, property.Name, columnName, Properties);
		}
		#endregion
	}
}