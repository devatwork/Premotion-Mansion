using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Descriptors
{
	/// <summary>
	/// Describes a column.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "dateTimeColumn2")]
	public class DateTimeColumnDescriptor : ColumnDescriptor
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
			var column = new DateTimePropertyColumn(propertyName, columnName, Properties);
			column.Initialize(context);
			return column;
		}
		#endregion
	}
}