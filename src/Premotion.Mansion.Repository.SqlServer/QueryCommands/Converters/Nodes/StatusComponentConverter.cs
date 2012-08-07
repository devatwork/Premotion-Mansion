using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Specifications;
using Premotion.Mansion.Core.Data.Specifications.Nodes;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Converters.Nodes
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="StatusSpecification"/>s.
	/// </summary>
	public class StatusComponentConverter : SpecificationConverter<StatusSpecification>
	{
		#region Overrides of SpecificationConverter<StatusSpecification>
		/// <summary>
		/// Converts the given <paramref name="specification"/> into a statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="specification">The <see cref="Specification"/> which to convert.</param>
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected override void DoConvert(IMansionContext context, StatusSpecification specification, QueryCommand command)
		{
			// check for any
			if (specification.Status == NodeStatus.Any)
				return;

			var buffer = new StringBuilder("( ");

			// create the parameter for now, because we do not trust the database server time
			var nowParameterName = command.Command.AddParameter(DateTime.Now);

			// check statusses
			if ((specification.Status & NodeStatus.Draft) == NodeStatus.Draft)
				buffer.AppendFormat("([{0}].[approved] = 0 AND [{0}].[archived] = 0)", command.Schema.RootTable.Name);

			if ((specification.Status & NodeStatus.Staged) == NodeStatus.Staged)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[publicationDate] > @{2} AND [{0}].[archived] = 0)", command.Schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((specification.Status & NodeStatus.Published) == NodeStatus.Published)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[publicationDate] <= @{2} AND [{0}].[expirationDate] >= @{2} AND [{0}].[archived] = 0)", command.Schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((specification.Status & NodeStatus.Expired) == NodeStatus.Expired)
				buffer.AppendFormat("{1}([{0}].[approved] = 1 AND [{0}].[expirationDate] < @{2} AND [{0}].[archived] = 0)", command.Schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty, nowParameterName);

			if ((specification.Status & NodeStatus.Archived) == NodeStatus.Archived)
				buffer.AppendFormat("{1} [{0}].[archived] = 1", command.Schema.RootTable.Name, buffer.Length > 2 ? " OR " : string.Empty);

			// finish the query
			command.QueryBuilder.AppendWhere(buffer + " )");
		}
		#endregion
	}
}