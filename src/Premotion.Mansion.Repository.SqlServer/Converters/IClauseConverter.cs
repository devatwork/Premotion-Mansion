using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Maps a <see cref="NodeQueryClause"/> to a SQL statement.
	/// </summary>
	public interface IClauseConverter : ICandidate<NodeQueryClause>
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
		void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, NodeQueryClause clause);
		#endregion
	}
}