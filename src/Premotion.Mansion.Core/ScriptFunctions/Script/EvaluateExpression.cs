using System;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Script
{
	/// <summary>
	/// Evaluates an expression.
	/// </summary>
	[ScriptFunction("EvaluateExpression")]
	public class EvaluateExpression : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expressionScriptService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public EvaluateExpression(IExpressionScriptService expressionScriptService)
		{
			// validate arguments
			if (expressionScriptService == null)
				throw new ArgumentNullException("expressionScriptService");

			// set values
			this.expressionScriptService = expressionScriptService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Evaluates the <paramref name="expression"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="expression">The expression which to parse.</param>
		/// <returns>Returns the value of the evaluated <paramref name="expression"/>.</returns>
		public object Evaluate(IMansionContext context, string expression)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(expression))
				throw new ArgumentNullException("expression");

			// parse the expression
			var parsedExpression = expressionScriptService.Parse(context, new LiteralResource(expression));

			// return the result
			return parsedExpression.Execute<object>(context);
		}
		#endregion
		#region Private Fields
		private readonly IExpressionScriptService expressionScriptService;
		#endregion
	}
}