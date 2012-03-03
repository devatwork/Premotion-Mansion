using System;
using System.Data.SqlClient;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="ParentOfClause"/> into a SQL query statement.
	/// </summary>
	public class ParentOfClauseConverter : ClauseConverter<ParentOfClause>
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
		protected override void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, ParentOfClause clause)
		{
			// check the depth for any depth
			if (clause.Depth == null)
			{
				// check has parent
				if (!clause.ChildPointer.HasParent)
					queryBuilder.AppendWhere("1 = 0");
				else
				{
					// loop through all the parents
					var buffer = new StringBuilder();
					var currentPointer = clause.ChildPointer;
					do
					{
						currentPointer = currentPointer.Parent;
						buffer.AppendFormat("@{0},", command.AddParameter(currentPointer.Id));
					} while (currentPointer.HasParent);

					// append the query
					queryBuilder.AppendWhere("[{1}].[id] IN ({0})", buffer.Trim(), schema.RootTable.Name);
				}
			}
			else
			{
				// determine the depth
				var depth = clause.Depth.Value < 0 ? Math.Abs(clause.Depth.Value) : clause.ChildPointer.Depth - clause.Depth.Value - 1;

				// check depth
				if (depth < 0 || depth >= clause.ChildPointer.Depth - 1)
					throw new IndexOutOfRangeException("The index is outside the bound of the pointer.");

				// append the query
				queryBuilder.AppendWhere("[{1}].[id] = @{0}", command.AddParameter(clause.ChildPointer.Pointer[depth]), schema.RootTable.Name);
			}
		}
		#endregion
	}
}