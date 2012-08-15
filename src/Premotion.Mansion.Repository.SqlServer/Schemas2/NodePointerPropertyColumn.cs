using System;
using System.Data;
using Premotion.Mansion.Core;
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
		#region Overrides of Column
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
		{
			queryBuilder.AddColumnValue("name", newPointer.Name.Trim(), DbType.String);
			queryBuilder.AddColumnValue("type", newPointer.Type.Trim(), DbType.String);
			queryBuilder.AddColumnValue("depth", newPointer.Depth, DbType.Int32);

			queryBuilder.AddColumnValue("parentId", newPointer.HasParent ? (object) newPointer.Parent.Id : null, DbType.Int32);
			queryBuilder.AddColumnValue("parentPointer", newPointer.HasParent ? newPointer.Parent.PointerString + NodePointer.PointerSeparator : null, DbType.String);
			queryBuilder.AddColumnValue("parentPath", newPointer.HasParent ? newPointer.Parent.PathString + NodePointer.PathSeparator : null, DbType.String);
			queryBuilder.AddColumnValue("parentStructure", newPointer.HasParent ? newPointer.Parent.StructureString + NodePointer.StructureSeparator : null, DbType.String);
		}
		#endregion
	}
}