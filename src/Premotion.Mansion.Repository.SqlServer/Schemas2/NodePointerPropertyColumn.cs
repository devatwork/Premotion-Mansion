using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represens a <see cref="NodePointer"/> property column.
	/// </summary>
	public class NodePointerPropertyColumn : PropertyColumn
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="properties"></param>
		/// <param name="table">The <see cref="Table"/>.</param>
		public NodePointerPropertyColumn(string propertyName, string columnName, IPropertyBag properties, Table table) : base(propertyName, columnName, properties)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add marker columns
			table.Add(new DummyColumn("name"));
			table.Add(new DummyColumn("type"));
			table.Add(new DummyColumn("depth"));
			table.Add(new DummyColumn("parentId"));
			table.Add(new DummyColumn("parentPointer"));
			table.Add(new DummyColumn("parentPath"));
			table.Add(new DummyColumn("parentStructure"));
		}
		#endregion
	}
}