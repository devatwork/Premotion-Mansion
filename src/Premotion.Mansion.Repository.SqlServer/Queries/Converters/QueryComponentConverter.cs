using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Repository.SqlServer.Queries.Converters
{
	/// <summary>
	/// Base type for all <see cref="IQueryComponentConverter"/>s.
	/// </summary>
	/// <typeparam name="TComponentType">The type of component this converter converts.</typeparam>
	[Exported(typeof (IQueryComponentConverter))]
	public abstract class QueryComponentConverter<TComponentType> : IQueryComponentConverter where TComponentType : QueryComponent
	{
		#region Conversion Methods
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Convert(IMansionContext context, QueryComponent component, QueryCommand command)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (component == null)
				throw new ArgumentNullException("component");
			if (command == null)
				throw new ArgumentNullException("command");

			// invoke template method
			DoConvert(context, (TComponentType) component, command);
		}
		/// <summary>
		/// Converts the given <paramref name="component"/> into an statement.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="component">The <see cref="QueryComponent"/> which to convert.</param>
		/// <param name="command">The <see cref="QueryCommand"/>.</param>
		protected abstract void DoConvert(IMansionContext context, TComponentType component, QueryCommand command);
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
			return subject is TComponentType ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
	}
}