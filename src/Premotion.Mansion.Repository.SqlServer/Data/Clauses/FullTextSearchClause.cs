using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;

namespace Premotion.Mansion.Repository.SqlServer.Data.Clauses
{
	/// <summary>
	/// Implements the search clause.
	/// </summary>
	public class FullTextSearchClause : NodeQueryClause
	{
		#region Nested type: FullTextSearchClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "WhereClause" />.
		/// </summary>
		public class FullTextSearchClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public FullTextSearchClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// Interprets the input.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext" />.</param>
			/// <param name="input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				// check for search
				string where;
				if (input.TryGetAndRemove(context, "fts", out where) && !string.IsNullOrEmpty(where))
					yield return new FullTextSearchClause(where);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a search clause.
		/// </summary>
		/// <param name="query">The search query.</param>
		public FullTextSearchClause(string query)
		{
			// validate arguments
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query");

			// set value
			Query = query;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the custom where query.
		/// </summary>
		public string Query { get; private set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("fts:{0}", Query);
		}
		#endregion
	}
}