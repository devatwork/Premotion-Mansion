using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the sort clause.
	/// </summary>
	public class SortClause : NodeQueryClause
	{
		#region Nested type: SortClauseInterpreter
		/// <summary>
		/// 	Interprets <see cref = "SortClause" />s.
		/// </summary>
		public class SortClauseInterpreter : QueryInterpreter
		{
			#region Constructors
			/// <summary>
			/// </summary>
			public SortClauseInterpreter() : base(10)
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

				// if there is no sort clause specified, sort by order
				string sort;
				if (input.TryGetAndRemove(context, "sort", out sort) && !string.IsNullOrEmpty(sort))
					yield return new SortClause(sort);
				else
					yield return SortClause.SortByOrder;
			}
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// 	Sorts by order.
		/// </summary>
		public static readonly SortClause SortByOrder = new SortClause("order ASC");
		#endregion
		#region Constructors
		/// <summary>
		/// 	Constructs a custom SQL where clause.
		/// </summary>
		/// <param name = "sortString">The where query.</param>
		public SortClause(string sortString)
		{
			// validate arguments
			if (string.IsNullOrEmpty(sortString))
				throw new ArgumentNullException("sortString");

			// set value
			Sorts = Sort.Parse(sortString);
		}
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the <see cref = "Sort" />s.
		/// </summary>
		public IEnumerable<Sort> Sorts { get; private set; }
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
			return string.Format("sort:{0}", string.Join("&", Sorts.Select(sort => sort.PropertyName + " " + (sort.Ascending ? "asc" : "desc"))));
		}
		#endregion
	}
}