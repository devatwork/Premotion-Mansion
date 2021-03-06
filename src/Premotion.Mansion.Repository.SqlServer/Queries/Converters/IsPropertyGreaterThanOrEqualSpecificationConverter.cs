using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Converts <see cref="IsPropertySmallerThanSpecification"/> to a statement.
	/// </summary>
	public class IsPropertyGreaterThanOrEqualSpecificationConverter : SpecificationConverter<IsPropertyGreaterThanOrEqualSpecification>
	{
		#region Overrides of SpecificationConverter<IsPropertyGreaterThanOrEqualSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, IsPropertyGreaterThanOrEqualSpecification specification, QueryCommandContext commandContext)
		{
			// get the table in which the column exists from the schema
			var pair = commandContext.Schema.FindTableAndColumn(specification.PropertyName);

			// add the table to the query
			commandContext.QueryBuilder.AddTable(context, pair.Table, commandContext.Command);

			// allow the table to map the value
			commandContext.QueryBuilder.AppendWhere("[{0}].[{1}] >= @{2}", pair.Table.Name, pair.Column.ColumnName, commandContext.Command.AddParameter(specification.Value));
		}
		#endregion
	}
}