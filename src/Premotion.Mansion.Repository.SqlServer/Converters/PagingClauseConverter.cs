using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="PagingClause"/> into a SQL query statement.
	/// </summary>
	public class PagingClauseConverter : ClauseConverter<PagingClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, PagingClause clause)
		{
			// TODO: make sure there is at least one sort

			// calculate
			var pageStart = ((clause.PageNumber - 1)*clause.PageSize) + 1;
			var pageEnd = pageStart + clause.PageSize - 1;

			// append part
			queryBuilder.OrderByEnabled = false;
			queryBuilder.SetPrefix("SELECT * FROM ( ");
			queryBuilder.AppendColumn("[{0}].*", schema.RootTable.Name);
			queryBuilder.AppendColumn("ROW_NUMBER() OVER( ORDER BY @orderBy@ ) AS _rowNumber");
			queryBuilder.SetPostfix(" ) AS Nodeset WHERE _rowNumber BETWEEN @{0} AND @{1}", command.AddParameter(pageStart), command.AddParameter(pageEnd));
		}
		#endregion
	}
}