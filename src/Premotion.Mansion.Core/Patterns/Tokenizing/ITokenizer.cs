using System.Collections.Generic;

namespace Premotion.Mansion.Core.Patterns.Tokenizing
{
	/// <summary>
	/// Represents a tokenizer.
	/// </summary>
	/// <typeparam name="TInput">The type of input this tokenizer tokenizes.</typeparam>
	/// <typeparam name="TToken">The type of output this tokenizer outputs.</typeparam>
	public interface ITokenizer<in TInput, out TToken>
	{
		#region Tokenize Methods
		/// <summary>
		/// Tokenizes the input.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input for this tokenizer.</param>
		/// <returns>Returns the tokens parsed from <paramref name="input"/>.</returns>
		IEnumerable<TToken> Tokenize(IMansionContext context, TInput input);
		#endregion
	}
}