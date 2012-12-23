using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Implements <see cref="BaseQueryEngine"/> using SQL-server.
	/// </summary>
	public class SqlServerQueryEngine : BaseQueryEngine
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="SqlServerQueryEngine"/>.
		/// </summary>
		/// <param name="repository">The <see cref="SqlServerRepository"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="repository"/> is null.</exception>
		public SqlServerQueryEngine(SqlServerRepository repository) : base(10)
		{
			// validate arguments
			if (repository == null)
				throw new ArgumentNullException("repository");

			// set values
			this.repository = repository;
		}
		#endregion
		#region Overrides of BaseQueryEngine
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected override Record DoRetrieveSingle(IMansionContext context, Query query)
		{
			return repository.RetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		protected override RecordSet DoRetrieve(IMansionContext context, Query query)
		{
			return repository.Retrieve(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected override Node DoRetrieveSingleNode(IMansionContext context, Query query)
		{
			return repository.RetrieveSingleNode(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected override Nodeset DoRetrieveNodeset(IMansionContext context, Query query)
		{
			return repository.RetrieveNodeset(context, query);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IMansionContext context, Query subject)
		{
			// TODO: check if all the components can be mapped
			return VoteResult.LowInterest;
		}
		#endregion
		#region Private Fields
		private readonly SqlServerRepository repository;
		#endregion
	}
}