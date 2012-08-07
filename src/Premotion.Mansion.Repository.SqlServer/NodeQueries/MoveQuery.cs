using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.NodeQueries
{
	/// <summary>
	/// Implements the move query.
	/// </summary>
	public class MoveQuery : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="moveCommand"></param>
		private MoveQuery(IDbCommand moveCommand)
		{
			// validate arguments
			if (moveCommand == null)
				throw new ArgumentNullException("moveCommand");

			// set values
			this.moveCommand = moveCommand;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Prepares an insert query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="connection">The connection.</param>
		/// <param name="transaction">The transaction.</param>
		/// <param name="schemaProvider"></param>
		/// <param name="pointer"></param>
		/// <param name="newParentPointer"></param>
		/// <returns></returns>
		public static MoveQuery Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (schemaProvider == null)
				throw new ArgumentNullException("schemaProvider");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");

			// retrieve the schema
			var schema = SchemaProvider.Resolve(context, pointer.Type);

			// create the commands
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.Transaction = transaction;
			var query = new StringBuilder();

			// calculate the new parent pointer
			var newPointer = NodePointer.ChangeParent(newParentPointer, pointer);

			// update the path
			query.AppendFormat(@"UPDATE [{0}] SET
							[parentPointer] = @newPointer + RIGHT( [parentPointer], LEN( [parentPointer] ) - @oldPointerLength ),
							[parentPath] = @newPath + RIGHT( [parentPath], LEN( [parentPath] ) - @oldPathLength ),
							[parentStructure] = @newStructure + RIGHT( [parentStructure], LEN( [parentStructure] ) - @oldStructureLength ),
							[depth] = (LEN(@newPointer + RIGHT( [parentPointer], LEN( [parentPointer] ) - @oldPointerLength )) - LEN(REPLACE(@newPointer + RIGHT( [parentPointer], LEN( [parentPointer] ) - @oldPointerLength ), '-', ''))) + 1
							WHERE ( [parentId] = @id OR [parentPointer] LIKE @oldPointer ); ", schema.RootTable.Name);
			query.AppendFormat(@"UPDATE [{0}] SET
							[parentId] = @newParentId,
							[parentPointer] = @newParentPointer,
							[parentPath] = @newParentPath,
							[parentStructure] = @newParentStructure,
							[depth] = (LEN(@newParentPointer) - LEN(REPLACE(@newParentPointer, '-', ''))) + 1
							WHERE [id] = @id ", schema.RootTable.Name);
			command.AddParameter(pointer.PointerString.Length + 1, "oldPointerLength");
			command.AddParameter(newPointer.PointerString + NodePointer.PointerSeparator, "newPointer");
			command.AddParameter(pointer.PathString.Length + 1, "oldPathLength");
			command.AddParameter(newPointer.PathString + NodePointer.PathSeparator, "newPath");
			command.AddParameter(pointer.StructureString.Length + 1, "oldStructureLength");
			command.AddParameter(newPointer.StructureString + NodePointer.StructureSeparator, "newStructure");
			command.AddParameter(pointer.PointerString + NodePointer.PointerSeparator, "oldPointer");
			command.AddParameter(newPointer.Id, "id");
			command.AddParameter(newParentPointer.Id, "newParentId");
			command.AddParameter(newParentPointer.PointerString + NodePointer.PointerSeparator, "newParentPointer");
			command.AddParameter(newParentPointer.PathString + NodePointer.PathSeparator, "newParentPath");
			command.AddParameter(newParentPointer.StructureString + NodePointer.StructureSeparator, "newParentStructure");

			// execute
			command.CommandText = query.ToString();

			// assemble the query
			return new MoveQuery(command);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the update command.
		/// </summary>
		public void Execute()
		{
			moveCommand.ExecuteNonQuery();
		}
		#endregion
		#region Dispose Implementation
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// cleanup
			moveCommand.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IDbCommand moveCommand;
		#endregion
	}
}