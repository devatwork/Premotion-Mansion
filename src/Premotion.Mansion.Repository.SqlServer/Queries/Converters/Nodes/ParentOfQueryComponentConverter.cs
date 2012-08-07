using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries.Specifications;
using Premotion.Mansion.Core.Data.Queries.Specifications.Nodes;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters.Nodes
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="ParentOfSpecification"/>s.
	/// </summary>
	public class ParentOfQueryComponentConverter : SpecificationConverter<ParentOfSpecification>
	{
		#region Overrides of SpecificationConverter<ParentOfSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, ParentOfSpecification specification, QueryCommandContext commandContext)
		{
			// check the depth for any depth
			if (specification.Depth == null)
			{
				// check has parent
				if (!specification.ChildPointer.HasParent)
					commandContext.QueryBuilder.AppendWhere("1 = 0");
				else
				{
					// loop through all the parents
					var buffer = new StringBuilder();
					var currentPointer = specification.ChildPointer;
					do
					{
						currentPointer = currentPointer.Parent;
						buffer.AppendFormat("@{0},", commandContext.Command.AddParameter(currentPointer.Id));
					} while (currentPointer.HasParent);

					// append the query
					commandContext.QueryBuilder.AppendWhere("[{1}].[id] IN ({0})", buffer.Trim(), commandContext.Schema.RootTable.Name);
				}
			}
			else
			{
				// determine the depth
				var depth = specification.Depth.Value < 0 ? Math.Abs(specification.Depth.Value) : specification.ChildPointer.Depth - specification.Depth.Value - 1;

				// check depth
				if (depth < 0 || depth >= specification.ChildPointer.Depth - 1)
					throw new IndexOutOfRangeException("The index is outside the bound of the pointer.");

				// append the query
				commandContext.QueryBuilder.AppendWhere("[{1}].[id] = @{0}", commandContext.Command.AddParameter(specification.ChildPointer.Pointer[depth]), commandContext.Schema.RootTable.Name);
			}
		}
		#endregion
	}
}