using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Represents a column of a <see cref="Table"/>.
	/// </summary>
	public abstract class Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		protected Column(string columnName) : this(columnName, columnName)
		{
		}
		/// <summary>
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="propertyName"></param>
		protected Column(string columnName, string propertyName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// set values
			ColumnName = columnName;
			PropertyName = propertyName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this column.
		/// </summary>
		public string ColumnName { get; private set; }
		/// <summary>
		/// Gets the name of the property to which this column maps.
		/// </summary>
		public string PropertyName { get; private set; }
		#endregion
		#region Statement Mapping Methods
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void ToWhereStatement(IMansionContext context, QueryCommandContext commandContext, IEnumerable<object> values)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (values == null)
				throw new ArgumentNullException("values");
			if (commandContext == null)
				throw new ArgumentNullException("commandContext");

			// invoke template method
			DoToWhereStatement(context, commandContext, values);
		}
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected virtual void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, IEnumerable<object> values)
		{
			// check if there are any values
			var valueArray = values.ToArray();
			if (valueArray.Length == 0)
			{
				commandContext.QueryBuilder.AppendWhere("1 = 0");
				return;
			}

			// get the table in which the column exists from the schema
			var pair = commandContext.Schema.FindTableAndColumn(PropertyName);

			// switch based on table type
			if (pair.Table is SingleValuePropertyTable)
				MapSingleValuePropertyTable(commandContext, pair, valueArray);
			else if (pair.Table is MultiValuePropertyTable)
				MapMultiValuePropertyTable(commandContext, pair, valueArray);
			else
				MapStandardTable(context, commandContext, pair, valueArray);
		}
		/// <summary>
		/// Maps the property to a single value property table.
		/// </summary>
		private static void MapSingleValuePropertyTable(QueryCommandContext commandContext, TableColumnPair pair, IEnumerable<object> values)
		{
			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in values)
				buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

			// append the query
			commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", commandContext.QueryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
		}
		/// <summary>
		/// Maps the property to a single value property table.
		/// </summary>
		private void MapMultiValuePropertyTable(QueryCommandContext commandContext, TableColumnPair pair, IEnumerable<object> values)
		{
			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in values)
				buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

			// append the query
			commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) AND [{1}].[name] = '{4}' )", commandContext.QueryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim(), PropertyName);
		}
		/// <summary>
		/// Maps the property to a standard table.
		/// </summary>
		private static void MapStandardTable(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
		{
			// add the table to the query
			commandContext.QueryBuilder.AddTable(context, pair.Table, commandContext.Command);

			// check for single or multiple values
			if (values.Count == 1)
				commandContext.QueryBuilder.AppendWhere(" [{0}].[{1}] = @{2}", pair.Table.Name, pair.Column.ColumnName, commandContext.Command.AddParameter(values[0]));
			else
			{
				// start the clause
				var buffer = new StringBuilder();
				buffer.AppendFormat("[{0}].[{1}] IN (", pair.Table.Name, pair.Column.ColumnName);

				// loop through all the values
				foreach (var value in values)
					buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

				// finish the clause
				commandContext.QueryBuilder.AppendWhere("{0})", buffer.Trim());
			}
		}
		#endregion
	}
}