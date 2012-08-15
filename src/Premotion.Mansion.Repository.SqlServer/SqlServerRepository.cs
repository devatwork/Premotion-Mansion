using System;
using System.Data;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.SqlServer.NodeQueries;
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
		public SqlServerRepository(IMansionContext context, string connectionString)
		{
			// valiate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException("connectionString");

			// set values
			this.connectionString = connectionString;
		}
		#endregion
		#region Implementation of IRepository
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns the node.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			// create the connection and command
			using (var connection = CreateConnection())
			using (var command = context.Nucleus.CreateInstance<SelectNodeCommand>())
			{
				// init the command with the query
				command.Prepare(context, connection, query);

				// execute the command
				return command.ExecuteSingle(context);
			}
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns></returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			// create the connection and command
			using (var connection = CreateConnection())
			using (var command = context.Nucleus.CreateInstance<SelectNodeCommand>())
			{
				// init the command with the query
				command.Prepare(context, connection, query);

				// execute the command
				return command.Execute(context);
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
			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			using (var command = context.Nucleus.CreateInstance<InsertNodeCommand>())
			{
				// init the command
				command.Prepare(context, connection, transaction, parent.Pointer, newProperties);

				// execute the command
				try
				{
					// execute the query
					var nodeId = command.Execute();

					// select the created node
					var selectQuery = new Query().Add(new IsPropertyEqualSpecification("id", nodeId));
					var createdNode = RetrieveSingleNode(context, selectQuery);

					// woohoo it worked!
					transaction.Commit();

					// return the created node
					return createdNode;
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
			using (var command = context.Nucleus.CreateInstance<UpdateNodeCommand>())
			{
				// init the command
				command.Prepare(context, connection, transaction, node, modifiedProperties);

				// execute the command
				try
				{
					// execute the query
					command.Execute();

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
			using (var command = context.Nucleus.CreateInstance<DeleteNodeCommand>())
			{
				// init the command
				command.Prepare(context, connection, transaction, pointer);

				// execute the command
				try
				{
					// execute the query
					command.Execute();

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
			var selectQuery = new Query().Add(new IsPropertyEqualSpecification("id", pointer.Id));
			return RetrieveSingleNode(context, selectQuery);
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
			var selectQuery = new Query();

			// build the query
			using (var connection = CreateConnection())
			using (var transaction = connection.BeginTransaction())
			{
				// retrieve the nodes
				// TODO: retrieve the nodes within the same transaction
				var nodeToCopy = RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", pointer.Id)));
				if (nodeToCopy == null)
					throw new ArgumentNullException(string.Format("Could not find node with pointer '{0}'", pointer));
				var targetParentNode = RetrieveSingleNode(context, new Query().Add(new IsPropertyEqualSpecification("id", targetParentPointer.Id)));
				if (targetParentNode == null)
					throw new ArgumentNullException(string.Format("Could not find node with pointer '{0}'", targetParentPointer));

				// create the copy query
				using (var copyQuery = CopyQuery.Prepare(context, connection, transaction, schemaProvider, nodeToCopy, targetParentNode))
				{
					try
					{
						// execute the query
						var copiedNodeId = copyQuery.Execute();

						selectQuery.Add(new IsPropertyEqualSpecification("id", copiedNodeId));

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
			return RetrieveSingleNode(context, selectQuery);
		}
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="IPropertyBag"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override IPropertyBag DoRetrieveSingle(IMansionContext context, Query query)
		{
			// create the connection and command
			using (var connection = CreateConnection())
			using (var command = context.Nucleus.CreateInstance<SelectCommand>())
			{
				// init the command with the query
				command.Prepare(context, connection, query);

				// execute the command
				return command.ExecuteSingle(context);
			}
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Dataset DoRetrieve(IMansionContext context, Query query)
		{
			// create the connection and command
			using (var connection = CreateConnection())
			using (var command = context.Nucleus.CreateInstance<SelectCommand>())
			{
				// init the command with the query
				command.Prepare(context, connection, query);

				// execute the command
				return command.Execute(context);
			}
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
		/// Executes the <paramref name="query"/> without a transaction.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The query wich to execute.</param>
		public void ExecuteWithoutTransaction(IMansionContext context, string query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// create the connection and the transaction
			using (var connection = CreateConnection())
			using (var command = connection.CreateCommand())
			{
				// prepare the command
				command.Connection = connection;
				command.CommandText = query;
				command.CommandType = CommandType.Text;

				// execute the command
				command.ExecuteNonQuery();
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
		private readonly SchemaProvider schemaProvider = new SchemaProvider();
		#endregion
	}
}