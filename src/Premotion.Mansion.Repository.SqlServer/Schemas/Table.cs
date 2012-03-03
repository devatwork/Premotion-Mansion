using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a table of a schema.
	/// </summary>
	public abstract class Table
	{
		#region Constructors
		/// <summary>
		/// Constructs a table.
		/// </summary>
		/// <param name="name"></param>
		protected Table(string name)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set values
			Name = name;
		}
		#endregion
		#region Column Methods
		/// <summary>
		/// Adds a column to this table.
		/// </summary>
		/// <param name="column">The column which to add.</param>
		public void AddColumn(Column column)
		{
			// validate arguments
			if (column == null)
				throw new ArgumentNullException("column");

			columns.Add(column);
		}
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		public void ToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (newPointer == null)
				throw new ArgumentNullException("newPointer");
			if (newProperties == null)
				throw new ArgumentNullException("newProperties");

			// invoke template method
			DoToInsertStatement(context, queryBuilder, newPointer, newProperties);
		}
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		protected abstract void DoToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties);
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		public void ToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (node == null)
				throw new ArgumentNullException("node");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");

			// invoke template method
			DoToUpdateStatement(context, queryBuilder, node, modifiedProperties);
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected abstract void DoToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties);
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		public void ToSyncStatement(MansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (bulkContext == null)
				throw new ArgumentNullException("bulkContext");
			if (nodes == null)
				throw new ArgumentNullException("nodes");

			// invoke template method
			DoToSyncStatement(context, bulkContext, nodes);
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		protected abstract void DoToSyncStatement(MansionContext context, BulkOperationContext bulkContext, List<Node> nodes);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this table.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the columns in this table.
		/// </summary>
		public IEnumerable<Column> Columns
		{
			get { return columns; }
		}
		#endregion
		#region Private Fields
		private readonly List<Column> columns = new List<Column>();
		#endregion
	}
}