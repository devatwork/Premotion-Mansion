using System;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for literal code parts.
	/// </summary>
	public class LiteralExpressionInterpreter : ExpressionPartInterpreter
	{
		#region Nested type: LiteralExpression
		/// <summary>
		/// Implements a literal expression.
		/// </summary>
		public class LiteralExpression : PhraseExpression
		{
			#region Constructors
			/// <summary>
			/// Constructs a literal expression with the content.
			/// </summary>
			/// <param name="content">The content.</param>
			public LiteralExpression(string content)
			{
				// validate arguments
				if (content == null)
					throw new ArgumentNullException("content");

				// set values
				this.content = content;
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

				return context.Nucleus.ResolveSingle<IConversionService>().Convert<TTarget>(context, content);
			}
			#endregion
			#region Private Fields
			private readonly string content;
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
			var isExpression = subject.Length > 2 && subject[0] == '{' && subject[subject.Length - 1] == '}';

			// refrain from voting expressions
			return isExpression ? VoteResult.Refrain : VoteResult.LowInterest;
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
			// generate the literal
			return new LiteralExpression(input);
		}
		#endregion
	}
}