using System;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns;
using Premotion.Mansion.Repository.SqlServer.Schemas;
using log4net;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Implements the copy query.
	/// </summary>
	public class CopyQuery : DisposableBase
	{
		#region Constants
		/// <summary>
		/// Adds
		/// </summary>
		private const string CopyPostfix = " - Copy";
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="insertQuery"></param>
		private CopyQuery(InsertQuery insertQuery)
		{
			// validate arguments
			if (insertQuery == null)
				throw new ArgumentNullException("insertQuery");

			// set values
			this.insertQuery = insertQuery;
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
		/// <param name="sourceNode"></param>
		/// <param name="targetParentNode"></param>
		/// <returns></returns>
		public static CopyQuery Prepare(MansionContext context, SqlConnection connection, SqlTransaction transaction, SchemaProvider schemaProvider, Node sourceNode, Node targetParentNode)
		{
			// validate arguments
			if (connection == null)
				throw new ArgumentNullException("connection");
			if (transaction == null)
				throw new ArgumentNullException("transaction");
			if (schemaProvider == null)
				throw new ArgumentNullException("schemaProvider");
			if (sourceNode == null)
				throw new ArgumentNullException("sourceNode");
			if (targetParentNode == null)
				throw new ArgumentNullException("targetParentNode");

			// get the properties of the new node
			var newProperties = new PropertyBag(sourceNode);

			// if the target pointer is the same as the parent of the source node, generate a new name to distinguish between the two.
			if (sourceNode.Pointer.Parent == targetParentNode.Pointer)
			{
				// get the name of the node
				var name = sourceNode.Get<string>(context, "name");

				// change the name to indicate the copied node
				newProperties.Set("name", name + CopyPostfix);
			}

			// create an insert query
			var insertQuery = InsertQuery.Prepare(context, connection, transaction, schemaProvider, targetParentNode.Pointer, newProperties);

			//create the copy query
			return new CopyQuery(insertQuery);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this query.
		/// </summary>
		public int Execute()
		{
			log.Info("Executing query");
			return insertQuery.Execute();
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
			insertQuery.Dispose();
		}
		#endregion
		#region Private Fields
		private static readonly ILog log = LogManager.GetLogger(typeof (DeleteQuery));
		private readonly InsertQuery insertQuery;
		#endregion
	}
}