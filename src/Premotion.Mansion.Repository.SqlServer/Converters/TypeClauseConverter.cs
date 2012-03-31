using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="TypeClause"/> into a SQL query statement.
	/// </summary>
	public class TypeClauseConverter : ClauseConverter<TypeClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, TypeClause clause)
		{
			// loop through all the types
			var buffer = new StringBuilder();
			foreach (var type in clause.Types)
				buffer.AppendFormat("@{0},", command.AddParameter(type.Name));

			// append the query
			queryBuilder.AppendWhere("[{1}].[type] IN ({0})", buffer.Trim(), schema.RootTable.Name);
		}
		#endregion
	}
}