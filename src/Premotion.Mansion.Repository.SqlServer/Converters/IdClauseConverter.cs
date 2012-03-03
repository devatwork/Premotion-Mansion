using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="IdClause"/> into a SQL query statement.
	/// </summary>
	public class IdClauseConverter : ClauseConverter<IdClause>
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
		protected override void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, IdClause clause)
		{
			// append the query
			queryBuilder.AppendWhere(" [{1}].[id] = @{0}", command.AddParameter(clause.Id), schema.RootTable.Name);
		}
		#endregion
	}
}