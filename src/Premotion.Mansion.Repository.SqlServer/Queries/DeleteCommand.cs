using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the delete query.
	/// </summary>
	public class DeleteCommand : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DeleteCommand(ITypeService typeService)
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
		/// <param name="record"></param>
		/// <returns></returns>
		public void Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, Record record)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (record == null)
				throw new ArgumentNullException("record");

			// retrieve the schema
			var type = typeService.Load(context, record.Type);
			var schema = Resolver.Resolve(context, type);

			// create the commands
			command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.Transaction = transaction;

			// check if the record contains a pointer
			NodePointer pointer;
			if (record.TryGet(context, "pointer", out pointer))
			{
				command.CommandText += string.Format(@"DELETE FROM [{0}] WHERE [{0}].[parentPointer] LIKE @pointer;", schema.RootTable.Name);
				command.Parameters.AddWithValue("pointer", pointer.PointerString + NodePointer.PointerSeparator + "%");
			}

			command.CommandText += string.Format(@"DELETE FROM [{0}] WHERE [{0}].[id] = @id;", schema.RootTable.Name);
			command.Parameters.AddWithValue("id", record.Id);
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