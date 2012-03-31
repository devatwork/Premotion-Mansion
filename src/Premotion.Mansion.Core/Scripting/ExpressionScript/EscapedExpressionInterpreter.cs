using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for escaped expression code parts.
	/// </summary>
	public class EscapedExpressionInterpreter : ExpressionPartInterpreter
	{
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
			// generate the literal
			return new LiteralExpressionInterpreter.LiteralExpression("{" + input.Substring(2));
		}
		#endregion
	}
}