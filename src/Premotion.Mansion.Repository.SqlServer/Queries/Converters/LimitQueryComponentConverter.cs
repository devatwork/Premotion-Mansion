using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
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
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, LimitQueryComponent component, QueryCommandContext commandContext)
		{
			commandContext.QueryBuilder.SetLimit(component.Limit);
		}
		#endregion
	}
}