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
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected override void DoConvert(IMansionContext context, ChildOfSpecification specification, QueryCommand command)
		{
			switch (specification.Depth)
			{
				case null:
					command.QueryBuilder.AppendWhere("[{1}].[parentPointer] LIKE @{0}", command.Command.AddParameter(specification.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), command.Schema.RootTable.Name);
					break;
				case 1:
					command.QueryBuilder.AppendWhere("[{1}].[parentId] = @{0}", command.Command.AddParameter(specification.ParentPointer.Id), command.Schema.RootTable.Name);
					break;
				default:
					command.QueryBuilder.AppendWhere("[{2}].[parentPointer] LIKE @{0} AND [Nodes].[depth] = @{1}", command.Command.AddParameter(specification.ParentPointer.PointerString + NodePointer.PointerSeparator + "%"), command.Command.AddParameter(specification.Depth + specification.ParentPointer.Depth), command.Schema.RootTable.Name);
					break;
			}
		}
		#endregion
	}
}