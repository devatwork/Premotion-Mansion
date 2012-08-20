using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Repository.SqlServer.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Represents a virtual <see cref="Column"/>.
	/// </summary>
	public class VirtualColumn : Column
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		public VirtualColumn(string propertyName) : base(propertyName)
		{
		}
		#endregion
		#region Overrides of Column
		/// <summary>
		/// Constructs a WHERE statements on this column for the given <paramref name="values"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		/// <param name="pair">The <see cref="TableColumnPair"/>.</param>
		/// <param name="values">The values on which to construct the where statement.</param>
		protected override void DoToWhereStatement(IMansionContext context, QueryCommandContext commandContext, TableColumnPair pair, IList<object> values)
		{
			// add the table to the query
			commandContext.QueryBuilder.AddTable(context, pair.Table, commandContext.Command);

			// check for single or multiple values
			if (values.Count == 1)
				commandContext.QueryBuilder.AppendWhere(" [{0}].[{1}] = @{2}", pair.Table.Name, pair.Column.ColumnName, commandContext.Command.AddParameter(values[0]));
			else
			{
				// start the clause
				var buffer = new StringBuilder();
				buffer.AppendFormat("[{0}].[{1}] IN (", pair.Table.Name, pair.Column.ColumnName);

				// loop through all the values
				foreach (var value in values)
					buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

				// finish the clause
				commandContext.QueryBuilder.AppendWhere("{0})", buffer.Trim());
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="properties"></param>
		protected override void DoToInsertStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, IPropertyBag properties)
		{
			// do  nothing
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="record"> </param>
		/// <param name="modifiedProperties"></param>
		protected override void DoToUpdateStatement(IMansionContext context, ModificationQueryBuilder queryBuilder, Record record, IPropertyBag modifiedProperties)
		{
			// do nothing
		}
		#endregion
	}
}