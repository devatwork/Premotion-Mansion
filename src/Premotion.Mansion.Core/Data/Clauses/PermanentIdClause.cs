using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the query clause for permanent IDs.
	/// </summary>
	public class PermanentIdClause : NodeQueryClause
	{
		#region Nested type: PermanentIdClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "PermanentIdClause" />.
		/// </summary>
		public class PermanentIdClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public PermanentIdClauseInterpreter() : base(10)
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
				// check for id
				string permanentIds;
				if (input.TryGetAndRemove(context, "guid", out permanentIds) && !string.IsNullOrEmpty(permanentIds))
					yield return new PermanentIdClause(permanentIds);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs this query part with the specified ID.
		/// </summary>
		/// <param name = "permanentIds">The ID.</param>
		public PermanentIdClause(string permanentIds)
		{
			// validate arguments
			if (string.IsNullOrEmpty(permanentIds))
				throw new ArgumentNullException("permanentIds");

			// set value
			PermanentIds = permanentIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
			if (PermanentIds.Length == 0)
				throw new InvalidOperationException("Can not create a permenant ID clause without at least on permanent ID");
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the permanent IDs.
		/// </summary>
		public string[] PermanentIds { get; private set; }
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
			return string.Format("guid:{0}", string.Join(",", PermanentIds));
		}
		#endregion
	}
}