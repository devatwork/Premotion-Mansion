using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	/// <summary>
	/// Converts the <see cref="BaseTypeClause"/> into a SQL query statement.
	/// </summary>
	public class BaseTypeClauseConverter : ClauseConverter<BaseTypeClause>
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
		protected override void Map(IMansionContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, BaseTypeClause clause)
		{
			// loop through all the types
			var buffer = new StringBuilder();
			foreach (var typeName in clause.Types.SelectMany(type => new List<string>(type.GetInheritingTypes(context).Select(inhertingType => inhertingType.Name)) {type.Name}).Distinct(StringComparer.OrdinalIgnoreCase))
				buffer.AppendFormat("@{0},", command.AddParameter(typeName));

			// append the query
			queryBuilder.AppendWhere("[{1}].[type] IN ({0})", buffer.Trim(), schema.RootTable.Name);
		}
		#endregion
	}
}