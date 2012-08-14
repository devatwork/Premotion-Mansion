using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries.Mappers;

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
		#region Mapper Methods
		/// <summary>
		/// Gets the <see cref="IRecordMapper"/>s of this table.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="IRecordMapper"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public IEnumerable<IRecordMapper> GetRecordMappers(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// invoke template method
			return DoGetRecordMappers(context);
		}
		/// <summary>
		/// Gets the <see cref="IRecordMapper"/>s of this table.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="IRecordMapper"/>s.</returns>
		protected virtual IEnumerable<IRecordMapper> DoGetRecordMappers(IMansionContext context)
		{
			throw new NotSupportedException();
		}
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Generates a statement which joins this table to the given <paramref name="rootTable"/>/
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rootTable">The root <see cref="Table"/> to which to join this table.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <returns>Returns the join statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="rootTable"/> is null.</exception>
		public string ToJoinStatement(IMansionContext context, Table rootTable, SqlCommand command)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (rootTable == null)
				throw new ArgumentNullException("rootTable");
			if (command == null)
				throw new ArgumentNullException("command");

			// invoke template method
			return DoToJoinStatement(context, rootTable, command);
		}
		/// <summary>
		/// Generates a statement which joins this table to the given <paramref name="rootTable"/>/
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="rootTable">The root <see cref="Table"/> to which to join this table.</param>
		/// <param name="command">The <see cref="SqlCommand"/>.</param>
		/// <returns>Returns the join statement.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="rootTable"/> is null.</exception>
		protected virtual string DoToJoinStatement(IMansionContext context, Table rootTable, SqlCommand command)
		{
			return string.Format("INNER JOIN [{0}] ON [{0}].[id] = [{1}].[id]", Name, rootTable.Name);
		}
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		public void ToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
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
		protected virtual void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		public void ToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
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
		protected virtual void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		public void ToSyncStatement(IMansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
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
		protected virtual void DoToSyncStatement(IMansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
		{
			throw new NotSupportedException();
		}
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