using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class SingleValuePropertyTable : Table
	{
		#region Nested type: SingleValuePropertyColumn
		/// <summary>
		/// Implements <see cref="Column"/> for single value properties.
		/// </summary>
		private class SingleValuePropertyColumn : Column
		{
			#region constructors
			/// <summary>
			/// </summary>
			/// <param name="columnName"></param>
			public SingleValuePropertyColumn(string columnName) : base("value", columnName)
			{
			}
			#endregion
			#region Overrides of Column
			/// <summary>
			/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
			/// <param name="pair">The <see cref="TableColumnPair"/>.</param>
			/// <param name="values">The values on which to construct the where statement.</param>
			protected override void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
			{
				// assemble the properties
				var buffer = new StringBuilder();
				foreach (var value in values)
					buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

				// append the query
				commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", commandContext.QueryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="queryBuilder"></param>
			/// <param name="properties"></param>
			protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
			{
				throw new NotSupportedException();
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="queryBuilder"></param>
			/// <param name="record"> </param>
			/// <param name="modifiedProperties"></param>
			protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
			{
				throw new NotSupportedException();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="tableName"/>.
		/// </summary>
		/// <param name="tableName">The name of this table.</param>
		/// <param name="propertyName">The name of the property which to store.</param>
		public SingleValuePropertyTable(string tableName, string propertyName) : base(tableName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// add a column
			Add(new SingleValuePropertyColumn(propertyName));

			// set value
			PropertyName = propertyName;
		}
		#endregion
		#region Overrides of Table
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
		{
			// check if there are any properties
			var values = properties.Get(context, PropertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
			if (values.Length == 0)
				return;

			// loop through each value and write an insert statement
			foreach (var value in values)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// set column values
				valueModificationQuery.AddColumnValue("id", "@ScopeIdentity");
				valueModificationQuery.AddColumnValue("value", value, DbType.String);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
			}
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"> </param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
		{
			// check if the property is modified
			string rawModifiedValue;
			if (!modifiedProperties.TryGet(context, PropertyName, out rawModifiedValue))
				return;

			// get the current values
			var currentValues = GetCurrentValues(queryBuilder.Command, record).ToList();

			// check if there are new properties
			var modifiedValues = rawModifiedValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

			// get the deleted values
			var deletedValues = currentValues.Except(modifiedValues, StringComparer.OrdinalIgnoreCase);
			var newValues = modifiedValues.Except(currentValues, StringComparer.OrdinalIgnoreCase);

			// create identity parameter
			var idParameterName = queryBuilder.AddParameter("id", record.Id, DbType.Int32);

			// generate the delete statements
			foreach (var deletedValue in deletedValues)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// build clause
				var valueParameterName = valueModificationQuery.AddParameter("value", deletedValue, DbType.String);
				valueModificationQuery.AppendWhereClause("[id] = " + idParameterName + " AND [value] = " + valueParameterName);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToDeleteStatement(Name));
			}

			// generate the insert statements
			foreach (var newValue in newValues)
			{
				// build the query
				var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

				// set column values
				valueModificationQuery.AddColumnValue("id", idParameterName);
				valueModificationQuery.AddColumnValue("value", newValue, DbType.String);

				// append the query
				queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
			}
		}
		/// <summary>
		/// Generates an table sync statement for this table.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="bulkContext"></param>
		/// <param name="nodes"></param>
		protected override void DoToSyncStatement(IMansionContext context, BulkOperationContext bulkContext, List<Node> nodes)
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
				// check if there are any properties
				var values = node.Get(context, PropertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				if (values.Length == 0)
					continue;

				// loop through each value and write an insert statement
				foreach (var value in values)
				{
					var node1 = node;
					var value1 = value;
					bulkContext.Add(command =>
					                {
					                	command.CommandType = CommandType.Text;
					                	command.CommandText = string.Format("INSERT INTO [{0}] ([id], [value]) VALUES ({1}, @{2});", Name, node1.Pointer.Id, command.AddParameter(value1));
					                });
				}
			}
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the current values of this table.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="record"></param>
		/// <returns></returns>
		private IEnumerable<string> GetCurrentValues(IDbCommand command, Record record)
		{
			using (var selectCommand = command.Connection.CreateCommand())
			{
				selectCommand.CommandType = CommandType.Text;
				selectCommand.CommandText = string.Format("SELECT [value] FROM [{0}] WHERE [id] = '{1}'", Name, record.Id);
				selectCommand.Transaction = command.Transaction;
				using (var reader = selectCommand.ExecuteReader())
				{
					if (reader == null)
						throw new InvalidOperationException("Something terrible happened");

					while (reader.Read())
						yield return reader.GetValue(0).ToString();
				}
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the property which to store.
		/// </summary>
		private string PropertyName { get; set; }
		#endregion
	}
}