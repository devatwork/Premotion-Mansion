using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Converts <see cref="QueryComponent"/> into SQL statements.
	/// </summary>
	public interface IQueryComponentConverter : ICandidate<QueryComponent>
	{
		#region Conversion Methods
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		void Convert(IMansionContext context, QueryComponent component, QueryCommand command);
		#endregion
	}
}