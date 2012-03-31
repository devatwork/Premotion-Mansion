using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the query part which limits the result set.
	/// </summary>
	public class LimitClause : NodeQueryClause
	{
		#region Nested type: LimitClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "LimitClause" />.
		/// </summary>
		public class LimitClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public LimitClauseInterpreter() : base(10)
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

				// check the input
				string limitString;
				if (!input.TryGetAndRemove(context, "limit", out limitString) || string.IsNullOrEmpty(limitString) || !limitString.IsNumber())
					yield break;

				// get the values
				var conversionService = context.Nucleus.ResolveSingle<IConversionService>();
				var limit = conversionService.Convert<int>(context, limitString);

				// create the paging clause
				yield return new LimitClause(limit);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs this query part which limits the result set.
		/// </summary>
		/// <param name = "limit">The limit.</param>
		public LimitClause(int limit)
		{
			// set value
			Limit = limit;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the result set limit.
		/// </summary>
		public int Limit { get; private set; }
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
			return string.Format("limit:{0}", Limit);
		}
		#endregion
	}
}