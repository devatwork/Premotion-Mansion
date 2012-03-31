using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Conversion;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements <see cref="IScript"/> for expression phrases.
	/// </summary>
	public class Phrase : IExpressionScript
	{
		#region Add Methods
		/// <summary>
		/// Adds a new expression to this phrase.
		/// </summary>
		/// <param name="expression">The expression which to add.</param>
		public void Add(IExpressionScript expression)
		{
			// validate arguments
			if (expression == null)
				throw new ArgumentNullException("expression");

			// add to collection
			expressions.Add(expression);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public void Execute(IMansionContext context)
		{
			Execute<object>(context);
		}
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the result of this script expression.</returns>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public TResult Execute<TResult>(IMansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// check if there is just one expression
			if (expressions.Count == 1)
				return expressions[0].Execute<TResult>(context);

			// loop through all the clauses and execute them
			var buffer = new StringBuilder();
			foreach (var expression in expressions)
				buffer.Append(expression.Execute<string>(context));

			// return the converted values
			return context.Nucleus.ResolveSingle<IConversionService>().Convert<TResult>(context, buffer.ToString());
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the expressions which make up this expression.
		/// </summary>
		public IEnumerable<IExpressionScript> Expressions
		{
			get { return expressions; }
		}
		#endregion
		#region Private Fields
		private readonly IList<IExpressionScript> expressions = new List<IExpressionScript>();
		#endregion
	}
}