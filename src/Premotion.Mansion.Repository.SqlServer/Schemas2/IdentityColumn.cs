using System;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents an identity column
	/// </summary>
	public class IdentityColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public IdentityColumn(Table table) : base("id")
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add the dummy columns
			// TODO: factor out relation properties
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