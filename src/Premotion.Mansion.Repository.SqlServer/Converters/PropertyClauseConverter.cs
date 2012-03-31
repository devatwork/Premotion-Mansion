using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="PropertyClause"/> into a SQL query statement.
	/// </summary>
	public class PropertyClauseConverter : ClauseConverter<PropertyClause>
	{
		#region Map Methods
		/// <summary>
		/// Maps this clause to a SQL query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="clause">The clause.</param>
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, PropertyClause clause)
		{
			// check if there are any values
			if (clause.Values.Length == 0)
			{
				queryBuilder.AppendWhere("1 = 0");
				return;
			}

			// get the table in which the column exists from the schema
			var pair = schema.FindTableAndColumn(clause.Property);

			// switch based on table type
			if (pair.Table is SingleValuePropertyTable)
				MapSingleValuePropertyTable(clause, pair, command, queryBuilder);
			else if (pair.Table is MultiValuePropertyTable)
				MapMultiValuePropertyTable(clause, pair, command, queryBuilder);
			else
				MapStandardTable(clause, pair, queryBuilder, command);
		}
		/// <summary>
		/// Maps the property to a single value property table.
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="pair"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		private static void MapSingleValuePropertyTable(PropertyClause clause, TableColumnPair pair, SqlCommand command, SqlStringBuilder queryBuilder)
		{
			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in clause.Values)
				buffer.AppendFormat("@{0},", command.AddParameter(value));

			// append the query
			queryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", queryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
		}
		/// <summary>
		/// Maps the property to a single value property table.
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="pair"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		private static void MapMultiValuePropertyTable(PropertyClause clause, TableColumnPair pair, SqlCommand command, SqlStringBuilder queryBuilder)
		{
			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in clause.Values)
				buffer.AppendFormat("@{0},", command.AddParameter(value));

			// append the query
			queryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) AND [{1}].[name] = '{4}' )", queryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim(), clause.Property);
		}
		/// <summary>
		/// Maps the property to a standard table.
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="pair"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="command"></param>
		private static void MapStandardTable(PropertyClause clause, TableColumnPair pair, SqlStringBuilder queryBuilder, SqlCommand command)
		{
			// add the table to the query
			queryBuilder.AddTable(pair.Table);

			// check for single or multiple values
			if (clause.Values.Length == 1)
				queryBuilder.AppendWhere(" [{0}].[{1}] = @{2}", pair.Table.Name, pair.Column.ColumnName, command.AddParameter(clause.Values[0]));
			else
			{
				// start the clause
				var buffer = new StringBuilder();
				buffer.AppendFormat("[{0}].[{1}] IN (", pair.Table.Name, pair.Column.ColumnName);

				// loop through all the values
				foreach (var value in clause.Values)
					buffer.AppendFormat("@{0},", command.AddParameter(value));

				// finish the clause
				queryBuilder.AppendWhere("{0})", buffer.Trim());
			}
		}
		#endregion
	}
}