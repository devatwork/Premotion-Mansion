using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="PagingQueryComponent"/>s.
	/// </summary>
	public class PagingQueryComponentConverter : QueryComponentConverter<PagingQueryComponent>
	{
		#region Overrides of QueryComponentConverter<PagingQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, PagingQueryComponent component, QueryCommandContext commandContext)
		{
			// TODO: make sure there is at least one sort

			// calculate
			var pageStart = ((component.PageNumber - 1)*component.PageSize) + 1;
			var pageEnd = pageStart + component.PageSize - 1;

			// append part
			commandContext.QueryBuilder.OrderByEnabled = false;
			commandContext.QueryBuilder.SetPrefix("SELECT * FROM ( ");
			commandContext.QueryBuilder.AppendColumn("[{0}].*", commandContext.Schema.RootTable.Name);
			commandContext.QueryBuilder.AppendColumn("ROW_NUMBER() OVER(" + SqlStringBuilder.OrderByReplacePlaceholder + ") AS _rowNumber");
			commandContext.QueryBuilder.SetPostfix(" ) AS Nodeset WHERE _rowNumber BETWEEN @{0} AND @{1}", commandContext.Command.AddParameter(pageStart), commandContext.Command.AddParameter(pageEnd));
		}
		#endregion
	}
}