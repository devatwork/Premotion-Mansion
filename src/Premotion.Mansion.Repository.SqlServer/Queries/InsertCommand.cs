using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the insert query.
	/// </summary>
	public class InsertCommand : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="typeService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public InsertCommand(ITypeService typeService)
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
		/// <param name="properties"></param>
		/// <returns></returns>
		public void Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, IPropertyBag properties)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// get the values
			var typeName = properties.Get<string>(context, "type", null);
			if (string.IsNullOrWhiteSpace(typeName))
				throw new InvalidOperationException("A record must have a type");

			// retrieve the type
			var type = typeService.Load(context, typeName);

			// get the schema of the root type
			var schema = Resolver.Resolve(context, type);

			// set the full text property
			SqlServerUtilities.PopulateFullTextColumn(context, type, properties, properties);

			// create the commands
			command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.Transaction = transaction;

			// prepare the query
			var queryBuilder = new ModificationQueryBuilder(command);

			// loop through all the tables in the schema and let them prepare for insert
			foreach (var table in schema.Tables)
				table.ToInsertStatement(context, queryBuilder, properties);

			// finish the complete insert statement
			queryBuilder.AppendQuery("SELECT @ScopeIdentity");

			// set the command text
			command.CommandText = queryBuilder.ToStatement();
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the insert command.
		/// </summary>
		/// <returns>Returns the ID of the inserted record.</returns>
		public int Execute()
		{
			// check if the command is initialized properly
			CheckDisposed();
			if (command == null)
				throw new InvalidOperationException("The command is not prepared. Call the prepare method before calling execute.");

			return Convert.ToInt32(command.ExecuteScalar());
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