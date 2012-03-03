using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="PermanentIdClause"/> into a SQL query statement.
	/// </summary>
	public class PermanentIdClauseConverter : ClauseConverter<PermanentIdClause>
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
		protected override void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, PermanentIdClause clause)
		{
			// append the query
			if (clause.PermanentIds.Length == 1)
				queryBuilder.AppendWhere(" [{1}].[guid] = @{0}", command.AddParameter(clause.PermanentIds[0]), schema.RootTable.Name);
			else
			{
				// start the clause
				var buffer = new StringBuilder();
				buffer.AppendFormat("[{0}].[guid] IN (", schema.RootTable.Name);

				// loop through all the values
				foreach (var value in clause.PermanentIds)
					buffer.AppendFormat("@{0},", command.AddParameter(value));

				// finish the clause
				queryBuilder.AppendWhere("{0})", buffer.Trim());
			}
		}
		#endregion
	}
}