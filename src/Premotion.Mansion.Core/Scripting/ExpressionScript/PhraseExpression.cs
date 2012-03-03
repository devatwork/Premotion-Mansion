using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the base class for all phrase expressions.
	/// </summary>
	public abstract class PhraseExpression : IExpressionScript
	{
		#region Evaluate Methods
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public void Execute(MansionContext context)
		{
			Execute<object>(context);
		}
		/// <summary>
		/// Evaluates this expression.
		/// </summary>
		/// <typeparam name="TResult">The resulting type.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the result of the evaluation.</returns>
		public abstract TResult Execute<TResult>(MansionContext context);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the expressions which make up this phrase.
		/// </summary>
		public IEnumerable<IExpressionScript> Expressions
		{
			get { throw new NotSupportedException(); }
		}
		#endregion
	}
}