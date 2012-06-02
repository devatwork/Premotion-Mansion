using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Converters;
using Premotion.Mansion.Repository.SqlServer.Queries;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Implements <see cref="IRepository"/> for SQL Server.
	/// </summary>
	public class SqlServerRepository : RepositoryBase
	{
		#region Constructors
		/// <summary>
		/// Constructs an instance of the SQL Server Repository with the specified connection string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="converters"> </param>
		/// <param name="interpreters"> </param>
		public SqlServerRepository(IMansionContext context, string connectionString, IEnumerable<IClauseConverter> converters, IEnumerable<QueryInterpreter> interpreters)
		{
			// valiate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException("connectionString");
			if (converters == null)
				throw new ArgumentNullException("converters");
			if (interpreters == null)
				throw new ArgumentNullException("interpreters");

			// set values
			this.connectionString = connectionString;
			this.converters = converters.ToArray();
			this.interpreters = interpreters.ToArray();
		}
		#endregion
		#region Implementation of IRepository
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns>Returns the node.</returns>
		protected override Node DoRetrieveSingle(IMansionContext context, NodeQuery query)
		{
			// build the query
			using (var connection = CreateConnection())
			using (var selectQuery = SelectQuery.Prepare(context, connection, schemaProvider, query, converters))
			{
				// execute the query
				return selectQuery.ExecuteSingle(context);
			}
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The query on the node.</param>
		/// <returns></returns>
		protected override Nodeset DoRetrieve(IMansionContext context, NodeQuery query)
		{
			// build the query
			using (var connection = CreateConnection())
			using (var selectQuery = SelectQuery.Prepare(context, connection, schemaProvider, query, converters))
			{
				// execute the query
				return selectQuery.Execute(context);
			}
		}
		/// <summary>
		/// Creates a new node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="newProperties">The properties of the node which to create.</param>
		/// <returns>Returns the created nodes.</returns>
		protected override Node DoCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			// create a query to retrieve the new node
			var selectQuery = new NodeQuery();

			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var insertQuery = InsertQuery.Prepare(context, connection, transaction, schemaProvider, parent.Pointer, newProperties))
			{
				try
				{
					// execute the query
					var nodeId = insertQuery.Execute();

					selectQuery.AddRange(new[] {new IdClause(nodeId)});

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}

			// return the created node
			return RetrieveSingle(context, selectQuery);
		}
		/// <summary>
		/// Updates an existing node in this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be updated.</param>
		/// <param name="modifiedProperties">The properties which to update.</param>
		protected override void DoUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// get the modified properties
			modifiedProperties = PropertyBag.GetModifiedProperties(context, node, modifiedProperties);
			if (modifiedProperties.Count == 0)
				return;

			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var updateQuery = UpdateQuery.Prepare(context, connection, transaction, schemaProvider, node, modifiedProperties))
			{
				try
				{
					// execute the query
					updateQuery.Execute();

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}

			// merge the modified properties back into the node
			node.Merge(modifiedProperties);
		}
		/// <summary>
		/// Deletes an existing node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be deleted.</param>
		protected override void DoDelete(IMansionContext context, NodePointer pointer)
		{
			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var deleteQuery = DeleteQuery.Prepare(context, connection, transaction, schemaProvider, pointer))
			{
				try
				{
					// execute the query
					deleteQuery.Execute();

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}
		}
		/// <summary>
		/// Moves an existing node in this repository to a new parent node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be moved.</param>
		/// <param name="newParentPointer">The pointer to the parent to which the node is moved.</param>
		/// <returns>Returns the moved node.</returns>m
		protected override Node DoMove(IMansionContext context, NodePointer pointer, NodePointer newParentPointer)
		{
			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var moveQuery = MoveQuery.Prepare(context, connection, transaction, schemaProvider, pointer, newParentPointer))
			{
				try
				{
					// execute the query
					moveQuery.Execute();

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}

			// return the moved node
			var selectQuery = new NodeQuery {new IdClause(pointer.Id)};
			return RetrieveSingle(context, selectQuery);
		}
		/// <summary>
		/// Copies an existing node in this repository to a new node.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="pointer">The pointer to the node which will be copied.</param>
		/// <param name="targetParentPointer">The pointer to the parent to which the copied node is added.</param>
		/// <returns>Returns the copied node.</returns>
		protected override Node DoCopy(IMansionContext context, NodePointer pointer, NodePointer targetParentPointer)
		{
			// create a query to retrieve the new node
			var selectQuery = new NodeQuery();

			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			{
				// retrieve the nodes
				// TODO: retrieve the nodes within the same transaction
				var nodeToCopy = RetrieveSingle(context, new NodeQuery {new IdClause(pointer.Id)});
				if (nodeToCopy == null)
					throw new ArgumentNullException(string.Format("Could not find node with pointer '{0}'", pointer));
				var targetParentNode = RetrieveSingle(context, new NodeQuery {new IdClause(targetParentPointer.Id)});
				if (targetParentNode == null)
					throw new ArgumentNullException(string.Format("Could not find node with pointer '{0}'", targetParentPointer));

				// create the copy query
				using (var copyQuery = CopyQuery.Prepare(context, connection, transaction, schemaProvider, nodeToCopy, targetParentNode))
				{
					try
					{
						// execute the query
						var copiedNodeId = copyQuery.Execute();

						selectQuery.AddRange(new[] {new IdClause(copiedNodeId)});

						// woohoo it worked!
						transaction.Commit();
					}
					catch (Exception)
					{
						// something terrible happened, revert everything
						transaction.Rollback();
						throw;
					}
				}
			}

			// return the created node
			return RetrieveSingle(context, selectQuery);
		}
		/// <summary>
		/// Parses <paramref name="arguments" /> into a <see cref="NodeQuery" />.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to parse.</param>
		/// <returns>Returns the parsed query.</returns>
		protected override NodeQuery DoParseQuery(IMansionContext context, IPropertyBag arguments)
		{
			// interpret all the clauses and return the query
			var query = new NodeQuery();
			query.AddRange(interpreters.OrderBy(interpreter => interpreter.Priority).SelectMany(interpreter => interpreter.Interpret(context, arguments)));
			return query;
		}
		/// <summary>
		/// Starts this object. This methods must be called after the object has been created and before it is used.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoStart(IMansionContext context)
		{
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
		}
		#endregion
		#region Bulk Operation Methods
		///<summary>
		/// Allows client code to do a bulk load operation. During the bulk load all full-text updates and foreign key constraints are disabled.
		///</summary>
		///<param name="bulkOperation">The operation which to perform.</param>
		///<exception cref="ArgumentNullException">Thrown when <paramref name="bulkOperation"/> is null.</exception>
		public void BulkOperation(Action<BulkOperationContext> bulkOperation)
		{
			// validate arguments
			if (bulkOperation == null)
				throw new ArgumentNullException("bulkOperation");
			CheckDisposed();

			// create the connection and the transaction
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var bulkContext = new BulkOperationContext(connection, transaction))
			{
				try
				{
					// prepare the command
					bulkOperation(bulkContext);

					// execute the bulk operation
					bulkContext.Execute();

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes the <paramref name="query"/> within a transaction.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The query wich to execute.</param>
		public void ExecuteWithTransaction(IMansionContext context, string query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// create the connection and the transaction
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var command = connection.CreateCommand())
			{
				// prepare the command
				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandText = query;
				command.CommandType = CommandType.Text;

				// execute the command
				try
				{
					command.ExecuteNonQuery();

					// woohoo it worked!
					transaction.Commit();
				}
				catch (Exception)
				{
					// something terrible happened, revert everything
					transaction.Rollback();
					throw;
				}
			}
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Initializes a new connection.
		/// </summary>
		/// <returns>Returns the connection.</returns>
		private SqlConnection CreateConnection()
		{
			// create the connection
			var connection = new SqlConnection(connectionString);

			// open the connection
			connection.Open();

			return connection;
		}
		#endregion
		#region Private Fields
		private readonly string connectionString;
		private readonly IEnumerable<IClauseConverter> converters;
		private readonly IEnumerable<QueryInterpreter> interpreters;
		private readonly SchemaProvider schemaProvider = new SchemaProvider();
		#endregion
	}
}