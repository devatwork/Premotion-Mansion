using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;
using log4net;

namespace Premotion.Mansion.Repository.SqlServer.Queries
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
		public static MoveQuery Prepare(MansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, NodePointer pointer, NodePointer newParentPointer)
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
			command.Parameters.Add("oldPointerLength", SqlDbType.VarChar).Value = pointer.PointerString.Length + 1;
			command.Parameters.Add("newPointer", SqlDbType.VarChar).Value = newPointer.PointerString + NodePointer.PointerSeparator;
			command.Parameters.Add("oldPathLength", SqlDbType.VarChar).Value = pointer.PathString.Length + 1;
			command.Parameters.Add("newPath", SqlDbType.VarChar).Value = newPointer.PathString + NodePointer.PathSeparator;
			command.Parameters.Add("oldStructureLength", SqlDbType.VarChar).Value = pointer.StructureString.Length + 1;
			command.Parameters.Add("newStructure", SqlDbType.VarChar).Value = newPointer.StructureString + NodePointer.StructureSeparator;
			command.Parameters.Add("oldPointer", SqlDbType.VarChar).Value = pointer.PointerString + NodePointer.PointerSeparator;
			command.Parameters.Add("id", SqlDbType.Int).Value = newPointer.Id;
			command.Parameters.Add("newParentId", SqlDbType.Int).Value = newParentPointer.Id;
			command.Parameters.Add("newParentPointer", SqlDbType.VarChar).Value = newParentPointer.PointerString + NodePointer.PointerSeparator;
			command.Parameters.Add("newParentPath", SqlDbType.VarChar).Value = newParentPointer.PathString + NodePointer.PathSeparator;
			command.Parameters.Add("newParentStructure", SqlDbType.VarChar).Value = newParentPointer.StructureString + NodePointer.StructureSeparator;

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
			log.Info("Executing move query: " + moveCommand.CommandText);
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
		private static readonly ILog log = LogManager.GetLogger(typeof (DeleteQuery));
		private readonly IDbCommand moveCommand;
		#endregion
	}
}