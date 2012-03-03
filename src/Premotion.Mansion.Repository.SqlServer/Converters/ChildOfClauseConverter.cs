using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="ChildOfClause"/> into a SQL query statement.
	/// </summary>
	public class ChildOfClauseConverter : ClauseConverter<ChildOfClause>
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
		protected override void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, ChildOfClause clause)
		{
			switch (clause.Depth)
			{
				case null:
					queryBuilder.AppendWhere("[{1}].[parentPointer] LIKE @{0}", command.AddParameter(clause.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), schema.RootTable.Name);
					break;
				case 1:
					queryBuilder.AppendWhere("[{1}].[parentId] = @{0}", command.AddParameter(clause.ParentPointer.Id), schema.RootTable.Name);
					break;
				default:
					queryBuilder.AppendWhere("[{2}].[parentPointer] LIKE @{0} AND [Nodes].[depth] = @{1}", command.AddParameter(clause.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), command.AddParameter(clause.Depth + clause.ParentPointer.Depth), schema.RootTable.Name);
					break;
			}
		}
		#endregion
	}
}