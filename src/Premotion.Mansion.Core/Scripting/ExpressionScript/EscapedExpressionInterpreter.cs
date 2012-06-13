using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for escaped expression code parts.
	/// </summary>
	public class EscapedExpressionInterpreter : ExpressionPartInterpreter
	{
		#region Nested type: EscapedExpression
		/// <summary>
		/// Implements a literal expression.
		/// </summary>
		private class EscapedExpression : PhraseExpression
		{
			#region Constructors
			/// <summary>
			/// Constructs a literal expression with the content.
			/// </summary>
			/// <param name="escapedExpression">The content.</param>
			public EscapedExpression(IExpressionScript escapedExpression)
			{
				// validate arguments
				if (escapedExpression == null)
					throw new ArgumentNullException("escapedExpression");

				// set values
				this.escapedExpression = escapedExpression;
			}
			#endregion
			#region Evaluate Methods
			/// <summary>
			/// Evaluates this expression.
			/// </summary>
			/// <typeparam name="TTarget">The target type.</typeparam>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the result of the evaluation.</returns>
			public override TTarget Execute<TTarget>(IMansionContext context)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");

				// evaluate to string
				var parsedContent = escapedExpression.Execute<string>(context);

				return context.Nucleus.ResolveSingle<IConversionService>().Convert<TTarget>(context, "{" + parsedContent + "}");
			}
			#endregion
			#region Private Fields
			private readonly IExpressionScript escapedExpression;
			#endregion
		}
		#endregion
		#region Vote Methods
		/// <summary>
		/// Asks a voter to cast a vote on the subject.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IMansionContext context, string subject)
		{
			// check
			var isEscapedCodePart = subject.Length > 2 && subject[0] == '{' && subject[1] == '\\' && subject[subject.Length - 1] == '}';

			// alway refrain from voting
			return isEscapedCodePart ? VoteResult.Veto : VoteResult.Refrain;
		}
		#endregion
		#region Interpret Methods
		/// <summary>
		/// Interprets the input.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected override IExpressionScript DoInterpret(IMansionContext context, string input)
		{
			// get the exrepssion service
			var expressionService = context.Nucleus.ResolveSingle<IExpressionScriptService>();

			// parse the expression
			var escapedExpression = expressionService.Parse(context, new LiteralResource(input.Substring(2, input.Length - 3)));

			// return the literal
			return new EscapedExpression(escapedExpression);
		}
		#endregion
	}
}