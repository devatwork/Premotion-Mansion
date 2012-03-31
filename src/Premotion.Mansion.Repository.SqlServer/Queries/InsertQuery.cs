using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the insert query.
	/// </summary>
	public class InsertQuery : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="insertCommand"></param>
		private InsertQuery(IDbCommand insertCommand)
		{
			// validate arguments
			if (insertCommand == null)
				throw new ArgumentNullException("insertCommand");

			// set values
			this.insertCommand = insertCommand;
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
		/// <param name="parent"></param>
		/// <param name="newProperties"></param>
		/// <returns></returns>
		public static InsertQuery Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, NodePointer parent, IPropertyBag newProperties)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (schemaProvider == null)
				throw new ArgumentNullException("schemaProvider");
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");

			// get the values
			var name = newProperties.Get<string>(context, "name");
			if (string.IsNullOrWhiteSpace(name))
				throw new InvalidOperationException("The node must have a name");
			var type = newProperties.Get<string>(context, "type");
			if (string.IsNullOrWhiteSpace(type))
				throw new InvalidOperationException("The node must have a type");

			// retrieve the schema
			var schema = SchemaProvider.Resolve(context, type);

			// create the new pointer
			var newPointer = NodePointer.Parse(string.Join(NodePointer.PointerSeparator, new[] {parent.PointerString, 0.ToString()}), string.Join(NodePointer.StructureSeparator, new[] {parent.StructureString, type}), string.Join(NodePointer.PathSeparator, new[] {parent.PathString, name}));

			// create the commands
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.Transaction = transaction;

			// prepare the query
			var queryBuilder = new ModificationQueryBuilder(command);

			// loop through all the tables in the schema and let them prepare for insert
			foreach (var table in schema.Tables)
				table.ToInsertStatement(context, queryBuilder, newPointer, newProperties);

			// finish the complete insert statement
			queryBuilder.AppendQuery("SELECT @ScopeIdentity");

			// set the command text
			command.CommandText = queryBuilder.ToStatement();

			// assemble the query
			return new InsertQuery(command);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the insert command.
		/// </summary>
		/// <returns>Returns the ID of the inserted record.</returns>
		public int Execute()
		{
			return Convert.ToInt32(insertCommand.ExecuteScalar());
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
			insertCommand.Dispose();
		}
		#endregion
		#region Private Fieldse
		private readonly IDbCommand insertCommand;
		#endregion
	}
}