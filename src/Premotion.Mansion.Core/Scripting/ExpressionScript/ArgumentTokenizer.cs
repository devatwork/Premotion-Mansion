using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Patterns.Tokenizing;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements the argument tokenizer.
	/// </summary>
	public class ArgumentTokenizer : ITokenizer<string, string>
	{
		#region Tokenize Methods
		/// <summary>
		/// Tokenizes the input.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input for this tokenizer.</param>
		/// <returns>Returns the tokens parsed from <paramref name="input"/>.</returns>
		public IEnumerable<string> Tokenize(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			// loop through all the charachter in the input
			var buffer = new StringBuilder();
			var inLiteral = false;
			var inExpression = false;
			var nestingDepth = 0;
			foreach (var chararcter in input)
			{
				// check if current character is a separator charachter
				if (chararcter == ',' && !inLiteral && nestingDepth == 0)
				{
					// output a token
					if (buffer.Length > 0)
					{
						// check for expression
						if (inExpression)
						{
							inExpression = false;
							buffer.Append('}');
						}

						// return the token
						yield return buffer.ToString();
						buffer.Length = 0;
					}

					// continue parsing
					continue;
				}

				// check for literal block
				if (chararcter == '\'' && !inExpression)
				{
					// check if this is an end of a comment block
					if (inLiteral)
					{
						// return the token
						yield return buffer.ToString();
						buffer.Length = 0;
					}

					// toggle literal flag
					inLiteral = !inLiteral;

					// continue parsing
					continue;
				}

				// ignore white spaces outside control blocks
				if (Char.IsWhiteSpace(chararcter) && !inLiteral && !inExpression)
					continue;

				// check for expression start
				if (buffer.Length == 0 && !inLiteral)
				{
					// set the in expression flag and add expression character
					inExpression = true;
					buffer.Append('{');
				}

				// check for deeper nesting
				if (chararcter == '(' && inExpression)
					nestingDepth++;
				if (chararcter == ')' && inExpression)
					nestingDepth--;

				// add the character to the buffer
				buffer.Append(chararcter);
			}

			// check if there is a remainder in the buffer
			if (buffer.Length > 0)
			{
				// check for expression
				if (inExpression)
					buffer.Append('}');

				yield return buffer.ToString();
			}
		}
		#endregion
	}
}