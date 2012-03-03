using System;
using System.Data;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents an identity column
	/// </summary>
	public class IdentityColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public IdentityColumn(Table table) : base("identity")
		{
			// validate arguments
			if (table == null)
				throw new ArgumentNullException("table");

			// add the dummy columns
			table.AddColumn(new DummyColumn("name"));
			table.AddColumn(new DummyColumn("type"));
			table.AddColumn(new DummyColumn("depth"));
			table.AddColumn(new DummyColumn("parentId"));
			table.AddColumn(new DummyColumn("parentPointer"));
			table.AddColumn(new DummyColumn("parentPath"));
			table.AddColumn(new DummyColumn("parentStructure"));
		}
		#endregion
		#region ToStatement Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
		{
			queryBuilder.AddColumnValue("name", newPointer.Name.Trim(), DbType.String);
			queryBuilder.AddColumnValue("type", newPointer.Type.Trim(), DbType.String);
			queryBuilder.AddColumnValue("depth", newPointer.Depth, DbType.Int32);

			queryBuilder.AddColumnValue("parentId", newPointer.HasParent ? (object) newPointer.Parent.Id : null, DbType.Int32);
			queryBuilder.AddColumnValue("parentPointer", newPointer.HasParent ? newPointer.Parent.PointerString + NodePointer.PointerSeparator : null, DbType.String);
			queryBuilder.AddColumnValue("parentPath", newPointer.HasParent ? newPointer.Parent.PathString + NodePointer.PathSeparator : null, DbType.String);
			queryBuilder.AddColumnValue("parentStructure", newPointer.HasParent ? newPointer.Parent.StructureString + NodePointer.StructureSeparator : null, DbType.String);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// make sure the relational intgrety is not comprimised
			if (modifiedProperties.Names.Any(x => x.Equals("depth", StringComparison.OrdinalIgnoreCase) || x.Equals("parentId", StringComparison.OrdinalIgnoreCase) || x.Equals("parentPath", StringComparison.OrdinalIgnoreCase) || x.Equals("parentStructure", StringComparison.OrdinalIgnoreCase)))
				throw new InvalidOperationException("The relational properties can not be changed");

			//  add the id an pointer parameters
			var idParameterName = queryBuilder.AddParameter("id", node.Pointer.Id, DbType.Int32);
			var pointerParameterName = queryBuilder.AddParameter("pointer", node.Pointer.PointerString + "-%", DbType.String);

			// add the where clause
			queryBuilder.AppendWhereClause("[id] = " + idParameterName);

			// check if the name changed
			string newName;
			if (modifiedProperties.TryGetAndRemove(context, "name", out newName))
			{
				newName = newName.Trim();
				if (string.IsNullOrEmpty(newName))
					throw new InvalidOperationException("Can not update column name with empty string");
				if (newName.Contains(NodePointer.PathSeparator))
					throw new InvalidOperationException(string.Format("Name '{0}' contains invalid characters", newName));
				if (!node.Pointer.Name.Equals(newName))
				{
					// add the name column modification
					queryBuilder.AddColumnValue("name", newName, DbType.String);

					// update the paths
					var oldPathLengthParameterName = queryBuilder.AddParameter("oldPathLength", node.Pointer.PathString.Length + 1, DbType.String);
					var newPathParameterName = queryBuilder.AddParameter("newPath", NodePointer.Rename(node.Pointer, newName).PathString + NodePointer.PathSeparator, DbType.String);
					queryBuilder.AppendQuery(string.Format(@" UPDATE [Nodes] SET [parentPath] = {0} + RIGHT( [parentPath], LEN( [parentPath] ) - {1} ) WHERE ( [parentId] = {2} OR [parentPointer] LIKE {3} )", newPathParameterName, oldPathLengthParameterName, idParameterName, pointerParameterName));
				}
			}

			// check if the type changed
			string newType;
			if (modifiedProperties.TryGetAndRemove(context, "type", out newType))
			{
				newType = newType.Trim();
				if (string.IsNullOrEmpty(newType))
					throw new InvalidOperationException("Can not update column type with empty string");
				if (newType.Contains(NodePointer.StructureSeparator))
					throw new InvalidOperationException(string.Format("Type '{0}' contains invalid characters", newType));
				if (!string.IsNullOrEmpty(newType) && !node.Pointer.Type.Equals(newType))
				{
					// add the name column modification
					queryBuilder.AddColumnValue("type", newType, DbType.String);

					// update the structures
					var newStructureParameterName = queryBuilder.AddParameter("newStructure", NodePointer.ChangeType(node.Pointer, newType).StructureString + NodePointer.StructureSeparator, DbType.String);
					var oldStructureLengthParameterName = queryBuilder.AddParameter("oldStructureLength", node.Pointer.StructureString.Length + 1, DbType.Int32);
					queryBuilder.AppendQuery(string.Format("UPDATE [Nodes] SET [parentStructure] = {0} + RIGHT( [parentStructure], LEN( [parentStructure] ) - {1} ) WHERE ( [parentId] = {2} OR [parentPointer] LIKE {3} )", newStructureParameterName, oldStructureLengthParameterName, idParameterName, pointerParameterName));
				}
			}
		}
		#endregion
	}
}