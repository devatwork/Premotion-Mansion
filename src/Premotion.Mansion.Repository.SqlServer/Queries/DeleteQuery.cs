using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;
using log4net;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the update query.
	/// </summary>
	public class DeleteQuery : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="deleteCommand"></param>
		private DeleteQuery(IDbCommand deleteCommand)
		{
			// validate arguments
			if (deleteCommand == null)
				throw new ArgumentNullException("deleteCommand");

			// set values
			this.deleteCommand = deleteCommand;
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
		/// <returns></returns>
		public static DeleteQuery Prepare(MansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, NodePointer pointer)
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

			// retrieve the schema
			var schema = SchemaProvider.Resolve(context, pointer.Type);

			// create the commands
			var deleteCommand = connection.CreateCommand();
			deleteCommand.CommandType = CommandType.Text;
			deleteCommand.Transaction = transaction;

			deleteCommand.CommandText = string.Format(@"DELETE FROM [{0}] WHERE [{0}].[parentPointer] LIKE @pointer; DELETE FROM [{0}] WHERE [{0}].[id] = @id", schema.RootTable.Name);
			deleteCommand.Parameters.AddWithValue("pointer", pointer.PointerString + NodePointer.PointerSeparator + "%");
			deleteCommand.Parameters.AddWithValue("id", pointer.Id);

			// assemble the query
			return new DeleteQuery(deleteCommand);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the update command.
		/// </summary>
		public void Execute()
		{
			log.Info("Executing delete query: " + deleteCommand.CommandText);
			deleteCommand.ExecuteNonQuery();
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
			deleteCommand.Dispose();
		}
		#endregion
		#region Private Fields
		private static readonly ILog log = LogManager.GetLogger(typeof (DeleteQuery));
		private readonly IDbCommand deleteCommand;
		#endregion
	}
}