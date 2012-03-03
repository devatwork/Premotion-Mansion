using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the interpreter for <see cref="ReferenceExpression"/>.
	/// </summary>
	public class ReferenceExpressionInterpreter : ExpressionPartInterpreter
	{
		#region Nested type: ReferenceExpression
		/// <summary>
		/// Implements a reference expression, which references to an object on the stack.
		/// </summary>
		private class ReferenceExpression : PhraseExpression
		{
			#region Constructors
			/// <summary>
			/// Constructs a placeholder expression.
			/// </summary>
			/// <param name="name"></param>
			public ReferenceExpression(string name)
			{
				// validate arguments
				if (string.IsNullOrEmpty(name))
					throw new ArgumentNullException("name");

				// set values
				this.name = name;
			}
			#endregion
			#region Evaluate Methods
			/// <summary>
			/// Evaluates this expression.
			/// </summary>
			/// <param name="context">The <see cref="MansionContext"/>.</param>
			/// <returns>Returns the result of the evaluation.</returns>
			public override TTarget Execute<TTarget>(MansionContext context)
			{
				// validate argument
				if (context == null)
					throw new ArgumentNullException("context");

				// get the reference from the stack
				IPropertyBag reference;
				if (!context.Stack.TryPeek<IPropertyBag>(name, out reference))
					return default(TTarget);

				// check if the type is a wrapped object
				if (reference is PropertyBagAdapterFactory.IAdaptedObject)
					return PropertyBagAdapterFactory.GetOriginalObject<TTarget>(reference);

				// do nothing
				return context.Nucleus.Get<IConversionService>(context).Convert<TTarget>(context, reference);
			}
			#endregion
			#region Private Fields
			private readonly string name;
			#endregion
		}
		#endregion
		#region Vote Methods
		/// <summary>
		/// Asks a voter to cast a vote on the subject.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(MansionContext context, string subject)
		{
			// alway refrain from voting
			return subject.Length > 4 && subject[0] == '{' && subject[1] == '$' && subject[subject.Length - 1] == '}' ? VoteResult.MediumInterest : VoteResult.Refrain;
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
			var referenceName = input.Substring(2, input.Length - 3).Trim();
			if (referenceName.Length == 0)
				throw new InvalidOperationException("A reference must have a name");

			// generate the expression
			return new ReferenceExpression(referenceName);
		}
		#endregion
	}
}