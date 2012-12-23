using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Patterns.Prioritized;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Data
{
	/// <summary>
	/// Represents an engine which can execute the <see cref="Query"/>s.
	/// </summary>
	public abstract class BaseQueryEngine : ICandidate<Query>, IPrioritized
	{
		#region Constructors
		/// <summary>
		/// Constructs a query engine with the given <paramref name="priority"/>.
		/// </summary>
		/// <param name="priority">Determines the priority of this engine when resolving election ties. The higher the priority, earlier this object is executed.</param>
		protected BaseQueryEngine(int priority)
		{
			Priority = priority;
		}
		#endregion
		#region Record Methods
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		public Record RetrieveSingle(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// invoke template method
			return DoRetrieveSingle(context, query);
		}
		/// <summary>
		/// Retrieves a single record from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Record"/> or null when no result is found.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		protected abstract Record DoRetrieveSingle(IMansionContext context, Query query);
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		public RecordSet Retrieve(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// invoke template method
			return DoRetrieve(context, query);
		}
		/// <summary>
		/// Retrieves a <see cref="Dataset"/> from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Dataset"/> containing the results.</returns>
		protected abstract RecordSet DoRetrieve(IMansionContext context, Query query);
		#endregion
		#region Node Methods
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		public Node RetrieveSingleNode(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// invoke template method
			return DoRetrieveSingleNode(context, query);
		}
		/// <summary>
		/// Retrieves a single node from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a single <see cref="Node"/>.</returns>
		protected abstract Node DoRetrieveSingleNode(IMansionContext context, Query query);
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="query"/> is null.</exception>
		public Nodeset RetrieveNodeset(IMansionContext context, Query query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// invoke template method
			return DoRetrieveNodeset(context, query);
		}
		/// <summary>
		/// Retrieves multiple nodes from this repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The <see cref="Query"/> which to execute.</param>
		/// <returns>Returns a <see cref="Nodeset"/>.</returns>
		protected abstract Nodeset DoRetrieveNodeset(IMansionContext context, Query query);
		#endregion
		#region Implementation of ICandidate<in Query>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IMansionContext context, Query subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// invoke template method
			return DoVote(context, subject);
		}
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected abstract VoteResult DoVote(IMansionContext context, Query subject);
		#endregion
		#region Implementation of IPrioritized
		/// <summary>
		/// Gets the relative priority of this object. The higher the priority, earlier this object is executed.
		/// </summary>
		public int Priority { get; private set; }
		#endregion
	}
}