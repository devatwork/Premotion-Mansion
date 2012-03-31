using System;
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
		/// Creates the column as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public override Column Create(IMansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the column name
			var columnName = Properties.Get<string>(context, "columnName", null) ?? propertyName;

			// create the column
			var column = new BooleanPropertyColumn(propertyName, columnName, Properties);
			column.Initialize(context);
			return column;
		}
		#endregion
	}
}