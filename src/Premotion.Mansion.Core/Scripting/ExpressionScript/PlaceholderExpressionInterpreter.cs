using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for escaped expression code parts.
	/// </summary>
	public class PlaceholderExpressionInterpreter : ExpressionPartInterpreter
	{
		#region Vote Methods
		/// <summary>
		/// Asks a voter to cast a vote on the subject.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(MansionContext context, string subject)
		{
			// check
			var isEscapedCodePart = subject.Length > 3 && subject[0] == '{' && subject[subject.Length - 1] == '}';

			// alway refrain from voting
			return isEscapedCodePart ? VoteResult.LowestInterest : VoteResult.Refrain;
		}
		#endregion
		#region Interpret Methods
		/// <summary>
		/// Interprets the input.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected override IExpressionScript DoInterpret(MansionContext context, string input)
		{
			// get the values
			var name = input.Substring(1, input.Length - 2).Trim();
			if (name.Length == 0)
				throw new InvalidOperationException("A placeholder must have a name");

			// generate the expression
			return new PlaceholderExpression(name);
		}
		#endregion
	}
}