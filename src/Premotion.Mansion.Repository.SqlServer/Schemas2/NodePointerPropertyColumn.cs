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
			table.Add(new PropertyColumn("name", "name"));
			table.Add(new PropertyColumn("type", "type"));
			table.Add(new PropertyColumn("depth", "depth"));
			table.Add(new PropertyColumn("parentId", "parentId"));
			table.Add(new PropertyColumn("parentPointer", "parentPointer"));
			table.Add(new PropertyColumn("parentPath", "parentPath"));
			table.Add(new PropertyColumn("parentStructure", "parentStructure"));
		}
		#endregion
	}
}