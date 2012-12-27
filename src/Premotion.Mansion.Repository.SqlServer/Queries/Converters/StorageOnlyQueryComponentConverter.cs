using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Implements the <see cref="IQueryComponentConverter"/> for  <see cref="StorageOnlyQueryComponent"/>s.
	/// </summary>
	public class StorageOnlyQueryComponentConverter : QueryComponentConverter<StorageOnlyQueryComponent>
	{
		#region Overrides of QueryComponentConverter<StorageOnlyQueryComponent>
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="commandContext">The <see cref="QueryCommandContext"/>.</param>
		protected override void DoConvert(IMansionContext context, StorageOnlyQueryComponent component, QueryCommandContext commandContext)
		{
			// do nothing
		}
		#endregion
	}
}