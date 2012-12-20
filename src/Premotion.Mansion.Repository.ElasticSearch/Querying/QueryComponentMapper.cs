using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.ElasticSearch.Querying
{
	/// <summary>
	/// Base class for type <see cref="IQueryComponentMapper"/>s
	/// </summary>
	/// <typeparam name="TQueryComponent">The type of <see cref="QueryComponent"/> mapped by this converter.</typeparam>
	[Exported(typeof (IQueryComponentMapper))]
	public abstract class QueryComponentMapper<TQueryComponent> : IQueryComponentMapper where TQueryComponent : QueryComponent
	{
		#region Implementation of IQueryComponentConverter
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="component"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Map(IMansionContext context, Query query, QueryComponent component, SearchDescriptor search)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");
			if (component == null)
				throw new ArgumentNullException("component");
			if (search == null)
				throw new ArgumentNullException("search");

			// Invoke template method
			DoMap(context, query, (TQueryComponent) component, search);
		}
		/// <summary>
		/// Maps the given <paramref name="component"/> to the <paramref name="search"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="query">The source <see cref="Query"/> being mapped.</param>
		/// <param name="component">The source <see cref="QueryComponent"/> being mapped.</param>
		/// <param name="search">The <see cref="SearchDescriptor"/> to which to map <paramref name="component"/>.</param>
		protected abstract void DoMap(IMansionContext context, Query query, TQueryComponent component, SearchDescriptor search);
		#endregion
		#region Implementation of ICandidate<in QueryComponent>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IMansionContext context, QueryComponent subject)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// check if the subject can be mapped by this converter.
			return subject is TQueryComponent ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
	}
}