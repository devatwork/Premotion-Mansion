using System;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="StatusClause"/> into a SQL query statement.
	/// </summary>
	public class StatusClauseConverter : ClauseConverter<StatusClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, StatusClause clause)
		{
			// check for any
			if (clause.Status == NodeStatus.Any)
				return;

			var buffer = new StringBuilder("( ");

			// create the parameter for now, because we do not trust the database server time
			var nowParameterName = command.AddParameter(DateTime.Now);

			// check statusses
			if ((clause.Status & NodeStatus.Draft) == NodeStatus.Draft)
				buffer.AppendFormat("([{0}].[approved] = 0 AND [{0}].[archived] = 0)", schema.RootTable.Name);

			if ((clause.Status & NodeStatus.Staged) == NodeStatus.Staged)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[publicationDate] > @{2} AND [{0}].[archived] = 0)", schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((clause.Status & NodeStatus.Published) == NodeStatus.Published)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[publicationDate] <= @{2} AND [{0}].[expirationDate] >= @{2} AND [{0}].[archived] = 0)", schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((clause.Status & NodeStatus.Expired) == NodeStatus.Expired)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[expirationDate] < @{2} AND [{0}].[archived] = 0)", schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((clause.Status & NodeStatus.Archived) == NodeStatus.Archived)
				buffer.AppendFormat("{1} [{0}].[archived] = 1", schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty);

			// finish the query
			queryBuilder.AppendWhere(buffer + " )");
		}
		#endregion
	}
}