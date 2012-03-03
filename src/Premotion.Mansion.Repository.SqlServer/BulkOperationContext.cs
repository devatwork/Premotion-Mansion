using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Represents the context for bulk operations.
	/// </summary>
	public class BulkOperationContext : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// Constructs a bulk loading context.
		/// </summary>
		/// <param name="connection">The <see cref="SqlConnection"/>.</param>
		/// <param name="transaction">The <see cref="SqlTransaction"/>.</param>
		public BulkOperationContext(SqlConnection connection, SqlTransaction transaction)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");

			this.connection = connection;
			this.transaction = transaction;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Executes the commands within this context.
		/// </summary>
		public void Execute()
		{
			// execute the command
			foreach (var command in commandQueue)
				command.ExecuteNonQuery();
		}
		/// <summary>
		/// Adds a new <see cref="SqlCommand"/> to this bulk operation.
		/// </summary>
		/// <param name="prepareCommand">The method preparing the <see cref="SqlCommand"/>.</param>
		public void Add(Action<SqlCommand> prepareCommand)
		{
			// create a new command
			var command = connection.CreateCommand();
			command.Transaction = transaction;

			// prepare the command
			prepareCommand(command);

			// enqueue the command
			commandQueue.Enqueue(command);
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

			// clean up
			if (transaction != null)
				transaction.Dispose();
			if (connection != null)
				connection.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly Queue<SqlCommand> commandQueue = new Queue<SqlCommand>();
		private readonly SqlConnection connection;
		private readonly SqlTransaction transaction;
		#endregion
	}
}