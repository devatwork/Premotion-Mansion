using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="WhereClause"/> into a SQL query statement.
	/// </summary>
	public class WhereClauseConverter : ClauseConverter<WhereClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, WhereClause clause)
		{
			// we dont know what tables are used in where clause so add them all
			foreach (var table in schema.TypeTables)
				queryBuilder.AddTable(context, table, command);

			// append the query
			queryBuilder.AppendWhere(clause.Where);
		}
		#endregion
	}
}