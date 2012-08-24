using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters.Nodes
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="ChildOfSpecification"/>s.
	/// </summary>
	public class ChildOfQueryComponentConverter : SpecificationConverter<ChildOfSpecification>
	{
		#region Overrides of SpecificationConverter<ChildOfSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, ChildOfSpecification specification, QueryCommandContext commandContext)
		{
			switch (specification.Depth)
			{
				case null:
					commandContext.QueryBuilder.AppendWhere("[{1}].[parentPointer] LIKE @{0}", commandContext.Command.AddParameter(specification.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), commandContext.Schema.RootTable.Name);
					break;
				case 1:
					commandContext.QueryBuilder.AppendWhere("[{1}].[parentId] = @{0}", commandContext.Command.AddParameter(specification.ParentPointer.Id), commandContext.Schema.RootTable.Name);
					break;
				default:
					commandContext.QueryBuilder.AppendWhere("[{2}].[parentPointer] LIKE @{0} AND [Nodes].[depth] = @{1}", commandContext.Command.AddParameter(specification.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), commandContext.Command.AddParameter(specification.Depth + specification.ParentPointer.Depth), commandContext.Schema.RootTable.Name);
					break;
			}
		}
		#endregion
	}
}