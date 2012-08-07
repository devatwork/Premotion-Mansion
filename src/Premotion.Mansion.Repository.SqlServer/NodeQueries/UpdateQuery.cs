using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.NodeQueries
{
	/// <summary>
	/// Implements the update query.
	/// </summary>
	public class UpdateQuery : DisposableBase
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="updateCommand"></param>
		private UpdateQuery(IDbCommand updateCommand)
		{
			// validate arguments
			if (updateCommand == null)
				throw new ArgumentNullException("updateCommand");

			// set values
			this.updateCommand = updateCommand;
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
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		/// <returns></returns>
		public static UpdateQuery Prepare(IMansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (schemaProvider == null)
				throw new ArgumentNullException("schemaProvider");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");

			// set the modified date
			modifiedProperties.TrySet("modified", DateTime.Now);

			// retrieve the type
			var type = context.Nucleus.ResolveSingle<ITypeService>().Load(context, node.Pointer.Type);

			// retrieve the schema
			var schema = SchemaProvider.Resolve(context, type);

			// set the full text property
			SqlServerUtilities.PopulateFullTextColumn(context, type, modifiedProperties, node);

			// create the commandse
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.Transaction = transaction;

			// prepare the query
			var queryBuilder = new ModificationQueryBuilder(command);

			// loop through all the tables in the schema and let them prepare for update
			foreach (var table in schema.Tables)
				table.ToUpdateStatement(context, queryBuilder, node, modifiedProperties);

			// finish the insert statement
			command.CommandText = queryBuilder.ToStatement();

			// assemble the query
			return new UpdateQuery(command);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the update command.
		/// </summary>
		public void Execute()
		{
			updateCommand.ExecuteNonQuery();
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
			updateCommand.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly IDbCommand updateCommand;
		#endregion
	}
}