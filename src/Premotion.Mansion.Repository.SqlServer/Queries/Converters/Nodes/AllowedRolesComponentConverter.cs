using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters.Nodes
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="AllowedRolesSpecification"/>s.
	/// </summary>
	public class AllowedRolesComponentConverter : SpecificationConverter<AllowedRolesSpecification>
	{
		#region Overrides of SpecificationConverter<AllowedRolesSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, AllowedRolesSpecification specification, QueryCommandContext commandContext)
		{
			// check if there are no values
			if (specification.RoleIds.Length == 0)
			{
				commandContext.QueryBuilder.AppendWhere("1 = 0");
				return;
			}

			// get the table in which the column exists from the schema
			var pair = commandContext.Schema.FindTableAndColumn("allowedRoleGuids");

			// assemble the properties
			var buffer = new StringBuilder();
			foreach (var value in specification.RoleIds)
				buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(value));

			// append the query
			commandContext.QueryBuilder.AppendWhere(" [{0}].[id] IN ( SELECT [{1}].[id] FROM [{1}] WHERE [{1}].[{2}] IN ({3}) )", commandContext.Schema.RootTable.Name, pair.Table.Name, pair.Column.ColumnName, buffer.Trim());
		}
		#endregion
	}
}