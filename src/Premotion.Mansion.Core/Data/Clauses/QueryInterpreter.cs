using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Patterns.Interpreting;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Implements the base class for all  node query interpreters.
	/// </summary>
	[Exported]
	public abstract class QueryInterpreter : IInterpreter<MansionContext, IPropertyBag, IEnumerable<NodeQueryClause>>
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name = "priority"></param>
		protected QueryInterpreter(int priority)
		{
			// set values
			Priority = priority;
		}
		#endregion
		#region Interpret Methods
		/// <summary>
		/// 	Interprets the input.
		/// </summary>
		/// <param name = "context">The <see cref = "MansionContext" />.</param>
		/// <param name = "input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		public IEnumerable<NodeQueryClause> Interpret(MansionContext context, IPropertyBag input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			// template method
			return DoInterpret(context, input);
		}
		/// <summary>
		/// 	Interprets the input.
		/// </summary>
		/// <param name = "context">The <see cref = "MansionContext" />.</param>
		/// <param name = "input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected abstract IEnumerable<NodeQueryClause> DoInterpret(MansionContext context, IPropertyBag input);
		#endregion
		#region Properties
		/// <summary>
		/// 	Gets the priority of this interpreter.
		/// </summary>
		public int Priority { get; private set; }
		#endregion
	}
}