using System;
using System.Collections.Generic;
using System.Text;
using Premotion.Mansion.Core.Patterns.Tokenizing;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements a tokenizer which tokenizes on complete sections of a string.
	/// </summary>
	public class PhraseScriptTokenizer : ITokenizer<string, string>
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
			var inSection = false;
			for (var index = 0; index < input.Length; index++)
			{
				// get the character
				var character = input[index];

				// check if we are in a section
				if (inSection)
				{
					if (character == '}')
					{
						// clear the section flag
						inSection = false;

						// check if the separating token should not be removed
						buffer.Append(character);

						// check if there is a remainder in the buffer
						if (buffer.Length > 0)
						{
							yield return buffer.ToString();
							buffer.Length = 0;
						}

						continue;
					}
				}
					// check for start of a section
				else if (character == '{' && (index < input.Length - 1 && !char.IsWhiteSpace(input[index + 1])))
				{
					// set the section flag
					inSection = true;

					// check if there is a remainder in the buffer
					if (buffer.Length > 0)
					{
						yield return buffer.ToString();
						buffer.Length = 0;
					}
				}

				// add the character to the buffer
				buffer.Append(character);
			}

			// check if there is a remainder in the buffer
			if (buffer.Length > 0)
				yield return buffer.ToString();
		}
		#endregion
	}
}