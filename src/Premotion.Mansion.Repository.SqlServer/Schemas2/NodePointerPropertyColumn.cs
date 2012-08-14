using System;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represens a <see cref="NodePointer"/> property column.
	/// </summary>
	public class NodePointerPropertyColumn : Column
	{
		#region Constructors
		/// <summary>
		/// Constructs a column.
		/// </summary>
		/// <param name="table">The <see cref="Table"/>.</param>
		/// <param name="propertyName">The name of the property.</param>
		public NodePointerPropertyColumn(Table table, string propertyName) : base(propertyName)
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add marker columns
			table.Add(new VirtualColumn("name"));
			table.Add(new VirtualColumn("type"));
			table.Add(new VirtualColumn("depth"));
			table.Add(new VirtualColumn("parentId"));
			table.Add(new VirtualColumn("parentPointer"));
			table.Add(new VirtualColumn("parentPath"));
			table.Add(new VirtualColumn("parentStructure"));
		}
		#endregion
	}
}