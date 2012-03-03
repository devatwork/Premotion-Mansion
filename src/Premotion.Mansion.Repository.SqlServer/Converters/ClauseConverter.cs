using System;
using System.Data.SqlClient;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Repository.SqlServer.Schemas;

namespace Premotion.Mansion.Repository.SqlServer.Converters
{
	///<summary>
	/// Base class for all converters.
	///</summary>
	/// <typeparam name="TClauseType">The type of clause.</typeparam>
	[Exported]
	public abstract class ClauseConverter<TClauseType> : IClauseConverter where TClauseType : NodeQueryClause
	{
		#region Vote Methods
		/// <summary>
		/// Asks a voter to cast a vote on the subject.
		/// </summary>
		/// <param name="applicationContext">The application context.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		public VoteResult Vote(IContext applicationContext, NodeQueryClause subject)
		{
			// validate arguments
			if (applicationContext == null)
				throw new ArgumentNullException("applicationContext");
			if (subject == null)
				throw new ArgumentNullException("subject");

			// check if the subject can be mapped by this converter.
			return subject is TClauseType ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
		#region Map Methods
		/// <summary>
		/// Maps this clause to a SQL query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="clause">The clause.</param>
		public void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, NodeQueryClause clause)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (command == null)
				throw new ArgumentNullException("command");
			if (queryBuilder == null)
				throw new ArgumentNullException("queryBuilder");
			if (clause == null)
				throw new ArgumentNullException("clause");
			var typedClause = (TClauseType) clause;
			if (typedClause == null)
				throw new InvalidOperationException(string.Format("Can not map node query clause '{0}' to '{1}'.", clause.GetType(), typeof (TClauseType)));

			// invoke implementation
			Map(context, schema, command, queryBuilder, typedClause);
		}
		/// <summary>
		/// Maps this clause to a SQL query.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="command"></param>
		/// <param name="queryBuilder"></param>
		/// <param name="clause">The clause.</param>
		protected abstract void Map(IContext context, Schema schema, SqlCommand command, SqlStringBuilder queryBuilder, TClauseType clause);
		#endregion
	}
}