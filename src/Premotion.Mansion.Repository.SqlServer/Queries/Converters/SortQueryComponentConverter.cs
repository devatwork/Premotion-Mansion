using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="SortQueryComponent"/>s.
	/// </summary>
	public class SortQueryComponentConverter : QueryComponentConverter<SortQueryComponent>
	{
		#region Overrides of QueryComponentConverter<SortQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, SortQueryComponent component, QueryCommandContext commandContext)
		{
			// loop through all the sorts
			foreach (var sort in component.Sorts)
			{
				// ignore sort on _score which is handled by the FTS clause converter
				if ("_score".Equals(sort.PropertyName, StringComparison.OrdinalIgnoreCase))
				{
					commandContext.QueryBuilder.AppendOrderBy(string.Format("[FTS].[RANK] {0}", sort.Ascending ? "ASC" : "DESC"));
					continue;
				}

				// check for ordering by function
				if (sort.PropertyName.Contains("(") && sort.PropertyName.EndsWith(")"))
				{
					// append the sort
					commandContext.QueryBuilder.AppendOrderBy(string.Format("{0} {1}", sort.PropertyName, sort.Ascending ? "ASC" : "DESC"));
				}
				else
				{
					// get the table and the column
					var tableAndColumn = commandContext.Schema.FindTableAndColumn(sort.PropertyName);

					// add the table to the query
					commandContext.QueryBuilder.AddTable(context, tableAndColumn.Table, commandContext.Command);

					// append the sort
					commandContext.QueryBuilder.AppendOrderBy(string.Format("[{0}].[{1}] {2}", tableAndColumn.Table.Name, tableAndColumn.Column.ColumnName, sort.Ascending ? "ASC" : "DESC"));
				}
			}
		}
		#endregion
	}
}