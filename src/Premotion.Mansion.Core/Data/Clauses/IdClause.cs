using System.Collections.Generic;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the query part for IDs.
	/// </summary>
	public class IdClause : NodeQueryClause
	{
		#region Nested type: IdClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "IdClause" />.
		/// </summary>
		public class IdClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public IdClauseInterpreter() : base(10)
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
				int id;
				if (input.TryGetAndRemove(context, "id", out id))
					yield return new IdClause(id);

				// check for pointer
				string pointer;
				if (input.TryGetAndRemove(context, "pointer", out pointer))
					yield return new IdClause(NodePointer.Parse(pointer).Id);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs this query part with the specified ID.
		/// </summary>
		/// <param name = "id">The ID.</param>
		public IdClause(int id)
		{
			// set value
			Id = id;
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the ID.
		/// </summary>
		public int Id { get; private set; }
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
			return string.Format("id:{0}", Id);
		}
		#endregion
	}
}