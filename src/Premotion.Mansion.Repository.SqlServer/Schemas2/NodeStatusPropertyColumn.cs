using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represens a <see cref="NodeStatus"/> property column.
	/// </summary>
	public class NodeStatusPropertyColumn : PropertyColumn
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="properties"></param>
		/// <param name="table">The <see cref="Table"/>.</param>
		public NodeStatusPropertyColumn(string propertyName, string columnName, IPropertyBag properties, Table table) : base(propertyName, columnName, properties)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add marker columns
			table.Add(new DummyColumn("approved"));
			table.Add(new DummyColumn("publicationDate"));
			table.Add(new DummyColumn("expirationDate"));
			table.Add(new DummyColumn("archived"));
		}
		#endregion
	}
}