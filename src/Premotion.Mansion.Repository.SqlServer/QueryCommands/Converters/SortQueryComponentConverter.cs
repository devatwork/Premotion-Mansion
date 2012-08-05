using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Converters
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
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected override void DoConvert(IMansionContext context, SortQueryComponent component, QueryCommand command)
		{
			// loop through all the sorts
			foreach (var sort in component.Sorts)
			{
				// get the table and the column
				var tableAndColumn = command.Schema.FindTableAndColumn(sort.PropertyName);

				// add the table to the query
				command.QueryBuilder.AddTable(context, tableAndColumn.Table, command.Command);

				// append the query
				command.QueryBuilder.AppendOrderBy(string.Format("[{0}].[{1}] {2}", tableAndColumn.Table.Name, tableAndColumn.Column.ColumnName, sort.Ascending ? "ASC" : "DESC"));
			}
		}
		#endregion
	}
}