using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Repository.SqlServer.Queries.Specifications;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Converts <see cref="SqlWhereSpecification"/> to a statement.
	/// </summary>
	public class SqlWhereSpecificationConverter : SpecificationConverter<SqlWhereSpecification>
	{
		#region Overrides of SpecificationConverter<SqlWhereSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, SqlWhereSpecification specification, QueryCommandContext commandContext)
		{
			// we dont know what tables are used in where clause so add them all
			foreach (var table in commandContext.Schema.TypeTables)
				commandContext.QueryBuilder.AddTable(context, table, commandContext.Command);

			// append the query
			commandContext.QueryBuilder.AppendWhere(specification.Where);
		}
		#endregion
	}
}