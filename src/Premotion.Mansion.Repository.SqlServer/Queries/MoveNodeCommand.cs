using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas2;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the move query.
	/// </summary>
	public class MoveNodeCommand : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public MoveNodeCommand(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Prepares an insert query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="connection">The connection.</param>
		/// <param name="transaction">The transaction.</param>
		/// <param name="pointer"></param>
		/// <param name="newParentPointer"></param>
		/// <returns></returns>
		public void Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, NodePointer pointer, NodePointer newParentPointer)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (pointer == null)
				throw new ArgumentNullException("pointer");
			if (newParentPointer == null)
				throw new ArgumentNullException("newParentPointer");

			// retrieve the schema
			var type = typeService.Load(context, pointer.Type);
			var schema = Resolver.Resolve(context, type);

			// create the commands
			command = connection.CreateCommand();
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
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the update command.
		/// </summary>
		public void Execute()
		{
			// check if the command is initialized properly
			CheckDisposed();
			if (command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");

			command.ExecuteNonQuery();
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;
			if (command != null)
				command.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		private SqlCommand command;
		#endregion
	}
}