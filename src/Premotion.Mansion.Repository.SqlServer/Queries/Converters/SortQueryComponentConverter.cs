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
				// get the table and the column
				var tableAndColumn = commandContext.Schema.FindTableAndColumn(sort.PropertyName);

				// add the table to the query
				commandContext.QueryBuilder.AddTable(context, tableAndColumn.Table, commandContext.Command);

				// append the query
				commandContext.QueryBuilder.AppendOrderBy(string.Format("[{0}].[{1}] {2}", tableAndColumn.Table.Name, tableAndColumn.Column.ColumnName, sort.Ascending ? "ASC" : "DESC"));
			}
		}
		#endregion
	}
}