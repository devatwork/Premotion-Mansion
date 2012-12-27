using System.Linq;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for function code parts.
	/// </summary>
	public class FunctionExpressionInterpreter : ExpressionPartInterpreter
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
			var openFunctionIndex = subject.IndexOf('(');
			if (openFunctionIndex == -1)
				return VoteResult.Refrain;
			var closeFunctionIndex = subject.LastIndexOf(')');
			if (closeFunctionIndex == -1)
				return VoteResult.Refrain;
			var isEscapedCodePart = subject.Length > 4 && subject[0] == '{' && openFunctionIndex < closeFunctionIndex && subject[subject.Length - 1] == '}';

			// alway refrain from voting
			return isEscapedCodePart ? VoteResult.HighInterest : VoteResult.Refrain;
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
			// get the function name
			var functionName = input.Substring(1, input.IndexOf('(') - 1);
			var rawArguments = input.Substring(functionName.Length + 2, input.Length - functionName.Length - 4).Trim();

			// parse the raw arguments
			var expressionScriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
			var argumentExpressions = (from rawArgument in argumentTokenizer.Tokenize(context, rawArguments)
			                           let interpreter = Election<ExpressionPartInterpreter, string>.Elect(context, expressionScriptService.Interpreters, rawArgument)
			                           select interpreter.Interpret(context, rawArgument)
			                          ).ToList();

			// get the function
			var function = context.Nucleus.ResolveSingle<FunctionExpression>(Constants.NamespaceUri, functionName);

			// initialize the function
			function.Initialize(context, argumentExpressions);

			// create function expression)
			return function;
		}
		#endregion
		#region Private Fields
		private readonly ArgumentTokenizer argumentTokenizer = new ArgumentTokenizer();
		#endregion
	}
}