using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a multi-value property table.
	/// </summary>
	public class MultiValuePropertyTable : Table
	{
		#region Nested type: MultiValuePropertyColumn
		/// <summary>
		/// Implements <see cref="Column"/> for single value properties.
		/// </summary>
		private class MultiValuePropertyColumn : Column
		{
			#region constructors
			/// <summary>
			/// </summary>
			/// <param name="columnName"></param>
			public MultiValuePropertyColumn(string columnName) : base(columnName)
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
				commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) AND [{1}].[name] = '{4}' )", commandContext.QueryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim(), PropertyName);
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="queryBuilder"></param>
			/// <param name="newPointer"></param>
			/// <param name="properties"></param>
			protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag properties)
			{
				throw new NotSupportedException();
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <param name="queryBuilder"></param>
			/// <param name="node"></param>
			/// <param name="modifiedProperties"></param>
			protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
			{
				throw new NotSupportedException();
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this table with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of this table.</param>
		public MultiValuePropertyTable(string name) : base(name)
		{
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds a property name ot this multi valued table.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>Returns this table.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is null.</exception>
		public MultiValuePropertyTable Add(string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// add a column
			Add(new MultiValuePropertyColumn(propertyName));

			// return this for chaining
			return this;
		}
		#endregion
		#region Overrides of Table
		/// <summary>
		/// Generates the insert statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="newPointer"></param>
		/// <param name="newProperties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, NodePointer newPointer, IPropertyBag newProperties)
		{
			// loop through all the properties
			foreach (var propertyName in propertyNames)
			{
				// check if there are any properties
				var values = newProperties.Get(context, propertyName, string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				if (values.Length == 0)
					continue;

				// loop through each value and write an insert statement
				foreach (var value in values)
				{
					// build the query
					var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

					// set column values
					valueModificationQuery.AddColumnValue("id", "@ScopeIdentity");
					valueModificationQuery.AddColumnValue("name", propertyName, DbType.String);
					valueModificationQuery.AddColumnValue("value", value, DbType.String);

					// append the query
					queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
				}
			}
		}
		/// <summary>
		/// Generates the update statement for this table.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="node"></param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Node node, IPropertyBag modifiedProperties)
		{
			// create identity parameter
			var idParameterName = queryBuilder.AddParameter("id", node.Pointer.Id, DbType.Int32);

			// loop through all the properties
			foreach (var propertyName in propertyNames)
			{
				// check if the property is modified
				string rawModifiedValue;
				if (!modifiedProperties.TryGet(context, propertyName, out rawModifiedValue))
					continue;

				// get the current values
				var currentValues = GetCurrentValues(queryBuilder.Command, node, propertyName).ToList();

				// check if there are new properties
				var modifiedValues = (rawModifiedValue ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

				// get the deleted values
				var deletedValues = currentValues.Except(modifiedValues, StringComparer.OrdinalIgnoreCase);
				var newValues = modifiedValues.Except(currentValues, StringComparer.OrdinalIgnoreCase);

				// create property parameter
				var propertyParameterName = queryBuilder.AddParameter(propertyName, propertyName, DbType.String);

				// generate the delete statements
				foreach (var deletedValue in deletedValues)
				{
					// build the query
					var valueModificationQuery = new ModificationQueryBuilder(queryBuilder);

					// build clause
					var valueParameterName = valueModificationQuery.AddParameter("value", deletedValue, DbType.String);
					valueModificationQuery.AppendWhereClause("[id] = " + idParameterName + " AND [name] = " + propertyParameterName + " AND [value] = " + valueParameterName);

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
					valueModificationQuery.AddColumnValue("name", propertyParameterName);
					valueModificationQuery.AddColumnValue("value", newValue, DbType.String);

					// append the query
					queryBuilder.AppendQuery(valueModificationQuery.ToInsertStatement(Name));
				}
			}
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the current values of this table.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="node"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		private IEnumerable<string> GetCurrentValues(IDbCommand command, Node node, string propertyName)
		{
			using (var selectCommand = command.Connection.CreateCommand())
			{
				// create the name parameter
				var nameParameter = command.CreateParameter();
				nameParameter.ParameterName = "name";
				nameParameter.DbType = DbType.String;
				nameParameter.Value = propertyName;
				nameParameter.Direction = ParameterDirection.Input;
				selectCommand.Parameters.Add(nameParameter);

				// assemble the command
				selectCommand.CommandType = CommandType.Text;
				selectCommand.CommandText = string.Format("SELECT [value] FROM [{0}] WHERE [id] = '{1}' AND [name] = @{2}", Name, node.Pointer.Id, nameParameter.ParameterName);
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
		#region Private Fields
		/// <summary>
		/// Gets the names of the properties which to store.
		/// </summary>
		private readonly List<string> propertyNames = new List<string>();
		#endregion
	}
}