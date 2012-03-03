using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="AllowedRolesClause"/> into a SQL query statement.
	/// </summary>
	public class AllowedRolesClauseConverter : ClauseConverter<AllowedRolesClause>
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
		protected override void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, AllowedRolesClause clause)
		{
			// check if there are no values
			if (clause.RoleIds.Length == 0)
			{
				queryBuilder.AppendWhere("1 = 0");
				return;
			}

			// get the table in which the column exists from the schema
			var pair = schema.FindTableAndColumn("allowedRoleGuids");

			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in clause.RoleIds)
				buffer.AppendFormat("@{0},", command.AddParameter(value));

			// append the query
			queryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", queryBuilder.RootTableName, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
		}
		#endregion
	}
}