using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the where clause.
	/// </summary>
	public class WhereClause : NodeQueryClause
	{
		#region Nested type: WhereClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "WhereClause" />.
		/// </summary>
		public class WhereClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public WhereClauseInterpreter() : base(10)
			{
			}
			#endregion
			#region Interpret Methods
			/// <summary>
			/// 	Interprets the input.
			/// </summary>
			/// <param name = "context">The <see cref = "IMansionContext" />.</param>
			/// <param name = "input">The input which to interpret.</param>
			/// <returns>Returns the interpreted result.</returns>
			protected override IEnumerable<NodeQueryClause> DoInterpret(IMansionContext context, IPropertyBag input)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (input == null)
					throw new ArgumentNullException("input");

				// check for parentPointer
				string where;
				if (input.TryGetAndRemove(context, "where", out where) && !string.IsNullOrEmpty(where))
					yield return new WhereClause(where);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs a custom where clause.
		/// </summary>
		/// <param name = "where">The where query.</param>
		public WhereClause(string where)
		{
			// validate arguments
			if (string.IsNullOrEmpty(where))
				throw new ArgumentNullException("where");

			// set value
			Where = where;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the custom where query.
		/// </summary>
		public string Where { get; private set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// 	Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </summary>
		/// <returns>
		/// 	A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("where:{0}", Where);
		}
		#endregion
	}
}