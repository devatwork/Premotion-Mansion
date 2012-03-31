using System;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for escaped expression code parts.
	/// </summary>
	public class PropertyExpressionInterpreter : ExpressionPartInterpreter
	{
		#region Nested type: PropertyExpression
		/// <summary>
		/// Implements a property expression.
		/// </summary>
		private class PropertyExpression : PhraseExpression
		{
			#region Constructors
			/// <summary>
			/// Constructs a property expression clause for the specified <paramref name="dataspace"/>.<paramref name="property"/>.
			/// </summary>
			/// <param name="dataspace">The name of the dataspace.</param>
			/// <param name="property">The name of the property.</param>
			public PropertyExpression(string dataspace, string property)
			{
				// validate arguments
				if (string.IsNullOrEmpty(dataspace))
					throw new ArgumentNullException("dataspace");
				if (string.IsNullOrEmpty(property))
					throw new ArgumentNullException("property");

				// set values
				Dataspace = dataspace;
				Property = property;
			}
			#endregion
			#region Evaluate Methods
			/// <summary>
			/// Evaluates this expression.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <returns>Returns the result of the evaluation.</returns>
			public override TTarget Execute<TTarget>(IMansionContext context)
			{
				// validate argument
				if (context == null)
					throw new ArgumentNullException("context");

				// check if the stack does not contain the dataspace
				IPropertyBag properties;
				if (!context.Stack.TryPeek(Dataspace, out properties))
					return default(TTarget);

				// return the result
				return properties.Get<TTarget>(context, Property);
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the dataspace name.
			/// </summary>
			public string Dataspace { get; private set; }
			/// <summary>
			/// Gets the property name.
			/// </summary>
			public string Property { get; private set; }
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
			var isEscapedCodePart = subject.Length > 4 && subject[0] == '{' && subject.Contains(".") && subject[subject.Length - 1] == '}';

			// alway refrain from voting
			return isEscapedCodePart ? VoteResult.MediumInterest : VoteResult.Refrain;
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
			// get the values
			var split = input.Substring(1, input.Length - 2).Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length != 2)
				throw new InvalidOperationException(string.Format("The property expression '{0}' must have format 'dataspace.property'", input));

			// generate the expression
			return new PropertyExpression(split[0].Trim(), split[1].Trim());
		}
		#endregion
	}
}