using System.Collections.Generic;
using System.Data;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a table into values of one specific type are stored.
	/// </summary>
	public class TypeTable : Table
	{
		#region Constructors
		/// <summary>
		/// Constructs a child type table.
		/// </summary>
		/// <param name="tableName"></param>
		public TypeTable(string tableName) : base(tableName)
		{
			// create the columns
			AddColumn(new JoinColumn());
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
		protected override void DoToInsertStatement(MansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToInsertStatement(context, tableModificationQuery, newPointer, newProperties);

			// if there are modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToInsertStatement(Name));
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(MansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// create a table modification query
			var tableModificationQuery = new ModificationQueryBuilder(queryBuilder);
			foreach (var column in Columns)
				column.ToUpdateStatement(context, tableModificationQuery, node, modifiedProperties);

			// if there are modified column add table modification query to the master query builder
			if (tableModificationQuery.HasModifiedColumns)
				queryBuilder.AppendQuery(tableModificationQuery.ToUpdateStatement(Name));
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		protected override void DoToSyncStatement(MansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
		{
			// start by clearing the table
			bulkContext.Add(command =>
			                {
			                	command.CommandType = CommandType.Text;
			                	command.CommandText = string.Format("TRUNCATE TABLE [{0}]", Name);
			                });

			// loop through all the properties
			foreach (var node in nodes)
			{
				// prepare the query
				var columnText = new StringBuilder();
				var valueText = new StringBuilder();

				// finish the query
				var node1 = node;
				bulkContext.Add(command =>
				                {
				                	// loop through all the columns
				                	foreach (var column in Columns)
				                		column.ToSyncStatement(context, command, node1, columnText, valueText);

				                	// construct the command
				                	command.CommandType = CommandType.Text;
				                	command.CommandText = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2});", Name, columnText.Trim(), valueText.Trim());
				                });
			}
		}
		#endregion
	}
}