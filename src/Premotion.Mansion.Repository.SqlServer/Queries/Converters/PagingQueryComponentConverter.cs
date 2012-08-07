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
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected override void DoConvert(IMansionContext context, PagingQueryComponent component, QueryCommand command)
		{
			// TODO: make sure there is at least one sort

			// calculate
			var pageStart = ((component.PageNumber - 1)*component.PageSize) + 1;
			var pageEnd = pageStart + component.PageSize - 1;

			// append part
			command.QueryBuilder.OrderByEnabled = false;
			command.QueryBuilder.SetPrefix("SELECT * FROM ( ");
			command.QueryBuilder.AppendColumn("[{0}].*", command.Schema.RootTable.Name);
			command.QueryBuilder.AppendColumn("ROW_NUMBER() OVER(" + SqlStringBuilder.OrderByReplacePlaceholder + ") AS _rowNumber");
			command.QueryBuilder.SetPostfix(" ) AS Nodeset WHERE _rowNumber BETWEEN @{0} AND @{1}", command.Command.AddParameter(pageStart), command.Command.AddParameter(pageEnd));
		}
		#endregion
	}
}