using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Repository.SqlServer.QueryCommands.Converters
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="LimitQueryComponent"/>s.
	/// </summary>
	public class LimitQueryComponentConverter : QueryComponentConverter<LimitQueryComponent>
	{
		#region Overrides of QueryComponentConverter<LimitQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected override void DoConvert(IMansionContext context, LimitQueryComponent component, QueryCommand command)
		{
			command.QueryBuilder.SetLimit("TOP {0}", component.Limit);
		}
		#endregion
	}
}