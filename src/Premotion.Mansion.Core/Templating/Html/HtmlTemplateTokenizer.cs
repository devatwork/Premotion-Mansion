using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Patterns.Tokenizing;

namespace Premotion.Mansion.Core.Templating.Html
{
	///<summary>
	/// Implements the tokinzer for HTML templates.
	///</summary>
	public class HtmlTemplateTokenizer : ITokenizer<string, string>
	{
		#region Constants
		/// <summary>
		/// Defines the start of a template section.
		/// </summary>
		public const string SectionStart = "<tpl:section";
		/// <summary>
		/// Defines the end of a templaet section.
		/// </summary>
		public const string SectionEnd = "</tpl:section>";
		#endregion
		#region Tokenize Methods
		/// <summary>
		/// Tokenizes the input.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="input">The input for this tokenizer.</param>
		/// <returns>Returns the tokens parsed from <paramref name="input"/>.</returns>
		public IEnumerable<string> Tokenize(IContext context, string input)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (input == null)
				throw new ArgumentNullException("input");

			// tokenize
			var sectionStartIndex = input.IndexOf(SectionStart, StringComparison.OrdinalIgnoreCase);
			while (sectionStartIndex > -1)
			{
				// get the index of the section end
				var sectionEndIndex = input.IndexOf(SectionEnd, sectionStartIndex, StringComparison.OrdinalIgnoreCase);
				if (sectionEndIndex == -1)
					throw new InvalidOperationException("Section not closed properly");

				// return the section
				yield return input.Substring(sectionStartIndex, sectionEndIndex + SectionEnd.Length - sectionStartIndex);

				// set the new start index
				sectionStartIndex = input.IndexOf(SectionStart, sectionEndIndex, StringComparison.OrdinalIgnoreCase);
			}
		}
		#endregion
	}
}