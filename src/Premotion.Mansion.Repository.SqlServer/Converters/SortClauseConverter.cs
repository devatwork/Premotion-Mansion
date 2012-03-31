using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="SortClause"/> into a SQL query statement.
	/// </summary>
	public class SortClauseConverter : ClauseConverter<SortClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, SortClause clause)
		{
			// loop through all the sorts
			foreach (var sort in clause.Sorts)
			{
				// get the table and the column
				var tableAndColumn = schema.FindTableAndColumn(sort.PropertyName);

				// add the table to the query
				queryBuilder.AddTable(tableAndColumn.Table);

				// append the query
				queryBuilder.AppendOrderBy(string.Format("[{0}].[{1}] {2}", tableAndColumn.Table.Name, tableAndColumn.Column.ColumnName, sort.Ascending ? "ASC" : "DESC"));
			}
		}
		#endregion
	}
}