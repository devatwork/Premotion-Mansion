using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Specified the used caching mechanism for the query.
	/// </summary>
	public class CacheClause : NodeQueryClause
	{
		#region Nested type: CacheClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "CacheClause" />.
		/// </summary>
		public class CacheClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public CacheClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "MansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(MansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				// check for the cache flag
				bool isCachable;
				if (!input.TryGetAndRemove(context, "cache", out isCachable))
					isCachable = true;

				// create and return the clause
				yield return new CacheClause(isCachable);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name = "isEnabled"></param>
		public CacheClause(bool isEnabled)
		{
			// set values
			IsEnabled = isEnabled;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets a flag indicating whether the result of this query can be cached or not.
		/// </summary>
		public bool IsEnabled { get; private set; }
		#endregion
		#region Overrides of NodeQueryClause
		/// <summary>
		/// 	Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		/// 	A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("cache-eanabled:{0}", IsEnabled);
		}
		#endregion
	}
}