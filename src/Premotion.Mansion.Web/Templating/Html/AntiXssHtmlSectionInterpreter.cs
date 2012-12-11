using System;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Patterns.Descriptors;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Templating.Html;
using Premotion.Mansion.Web.Templating.Html.Encoders;

namespace Premotion.Mansion.Web.Templating.Html
{
	/// <summary>
	/// Implements the interpreter for html sections which prevents XSS.
	/// </summary>
	public class AntiXssHtmlSectionInterpreter : SectionInterpreter
	{
		#region Constants
		/// <summary>
		/// Defines the start of a template section.
		/// </summary>
		private const string SectionStart = "<tpl:section";
		/// <summary>
		/// Defines the end of a templaet section.
		/// </summary>
		private const string SectionEnd = "</tpl:section>";
		/// <summary>
		/// Defines the end of a templaet section.
		/// </summary>
		private const string NamespaceUri = "http://schemas.premotion.nl/mansion/1.0/template.xsd";
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
			return subject.StartsWith(SectionStart, StringComparison.OrdinalIgnoreCase) && subject.EndsWith(SectionEnd, StringComparison.OrdinalIgnoreCase) ? VoteResult.HighInterest : VoteResult.Refrain;
		}
		#endregion
		#region Interpret Methods
		/// <summary>
		/// Interprets the input..
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected override Section DoInterpret(IMansionContext context, string input)
		{
			// get the header and content
			var headerEnd = input.IndexOf('>');
			var header = input.Substring(0, headerEnd) + " xmlns:tpl=\"" + NamespaceUri + "\"/>";
			var content = input.Substring(headerEnd + 1, input.Length - headerEnd - SectionEnd.Length - 1);

			// get the descriptor header
			var headerDescriptor = XmlDescriptorFactory<HtmlSectionHeaderDescriptor>.Create(context, header);

			// inject the encoders
			content = InjectEncoders(context, content);

			// parse the content
			var expressionScriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
			var expression = expressionScriptService.Parse(context, new LiteralResource(content.Trim()));

			// create the section
			return new Section(headerDescriptor.Properties, expression);
		}
		#endregion
		#region Inject Methods
		/// <summary>
		/// Injects the XSS encoders in the given <paramref name="content"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="content">The content in which to inject the encoders.</param>
		/// <returns>Returns the <paramref name="content"/> with the injectected encoders.</returns>
		private static string InjectEncoders(IMansionContext context, string content)
		{
			var inHtmlTag = false;
			var inHtmlTagBody = true;
			var inJavascriptTag = false;
			var inAttribute = false;
			var inExpresion = false;
			var output = new StringBuilder(content.Length);

			// loop over all the characters in the content
			var previousCharacter = char.MinValue;
			var quoteCharacter = char.MinValue;
			var buffer = new StringBuilder(content.Length);
			for (var index = 0; index < content.Length; index++)
			{
				// get the current character
				var currentCharacter = content[index];

				// check for expression end
				if (currentCharacter == '}' && inExpresion)
				{
					inExpresion = false;
				}
				else if (currentCharacter == '{' && !inExpresion)
				{
					inExpresion = true;
				}
				// check if we are exiting an attribute
				else if (currentCharacter == quoteCharacter && inAttribute && !inExpresion)
				{
					// write the encoded buffer to the output
					var attributeContent = buffer.ToString();
					buffer.Length = 0;

					// encode expressions in the content
					attributeContent = EncodeExpressions(context, AntiXssHtmlAttributeEncode.FunctionName, attributeContent);

					// append the encoded content to the output
					output.Append(attributeContent);

					// we are no  longer in an attribute
					inAttribute = false;
				}
					// check if we are entering an attribute
				else if (previousCharacter == '=' && (currentCharacter == '\'' || currentCharacter == '"') && inHtmlTag && !inExpresion)
				{
					// reset the buffer
					output.Append(buffer.ToString());
					buffer.Length = 0;

					// we are in an html tag
					inAttribute = true;
					quoteCharacter = currentCharacter;
				}
					// check if we are exiting a tag
				else if (currentCharacter == '>' && inHtmlTag && !inExpresion)
				{
					// reset the buffer
					output.Append(buffer.ToString());
					buffer.Length = 0;

					// reset the flag
					inHtmlTag = false;
					inHtmlTagBody = true;
				}
				else if (previousCharacter == '<' && currentCharacter == '/' && inHtmlTagBody && !inExpresion)
				{
					// get the content of the closed tag
					var tagContent = buffer.ToString();
					buffer.Length = 0;

					// encode expressions in the content
					tagContent = EncodeExpressions(context, inJavascriptTag ? AntiXssJavaScriptStringEncode.FunctionName : AntiXssHtmlEncode.FunctionName, tagContent);

					// append the encoded content to the output
					output.Append(tagContent);

					// reset flag
					inJavascriptTag = false;
					inHtmlTagBody = false;
				}
					// check if we are entering a script tag
				else if (currentCharacter == '<' && inHtmlTagBody && !inExpresion)
				{
					// get the content of the closed tag
					var tagContent = buffer.ToString();
					buffer.Length = 0;

					// encode expressions in the content
					tagContent = EncodeExpressions(context, inJavascriptTag ? AntiXssJavaScriptStringEncode.FunctionName : AntiXssHtmlEncode.FunctionName, tagContent);

					// append the encoded content to the output
					output.Append(tagContent);

					// reset flag
					inJavascriptTag = index + "script".Length < content.Length && content.IndexOf("script", index + 1, "script".Length, StringComparison.OrdinalIgnoreCase) == index + 1;
					inHtmlTag = true;
				}

				// append the current character to the buffer
				buffer.Append(currentCharacter);

				// set the previous character
				previousCharacter = currentCharacter;
			}

			// check for unbalanced expressions
			if (inExpresion)
				throw new InvalidOperationException(string.Format("Unbalanced script detected: '{0}'", content));

			// flush the buffer
			var bufferContent = buffer.ToString();
			if (inAttribute)
				bufferContent = EncodeExpressions(context, AntiXssHtmlAttributeEncode.FunctionName, bufferContent);
			if (inHtmlTagBody && inJavascriptTag)
				bufferContent = EncodeExpressions(context, AntiXssJavaScriptStringEncode.FunctionName, bufferContent);
			else if (inHtmlTagBody)
				bufferContent = EncodeExpressions(context, AntiXssHtmlEncode.FunctionName, bufferContent);
			output.Append(bufferContent);

			// return the content of the buffer
			return output.ToString();
		}
		/// <summary>
		/// Encodes the expression in the given <paramref name="input"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="encoderFunctionName">The name of the encoder function which to use.</param>
		/// <param name="input">The fragment in which to encode the expression.</param>
		/// <returns>Returns the encoded expressions.</returns>
		private static string EncodeExpressions(IMansionContext context, string encoderFunctionName, string input)
		{
			// create a buffer
			var buffer = new StringBuilder(input.Length);

			// loop over all the tokens
			var tokens = Tokenizer.Tokenize(context, input);
			foreach (var token in tokens)
			{
				// check if this is an expression
				if (token.Length <= 2 || token[0] != '{' || token[token.Length - 1] != '}')
				{
					// literal block, just add it to the buffer
					buffer.Append(token);
					continue;
				}

				// check for autoparses or escaped expressions
				if (token[1] == '@' || token[1] == '\\')
				{
					buffer.Append(token);
					continue;
				}

				// check for manually encoded expressions
				if (token[1] == '#')
				{
					buffer.Append('{');
					buffer.Append(token.Skip(2).ToArray());
					continue;
				}

				// check for placeholders
				if (token.Skip(1).Take(token.Length - 2).All(Char.IsLetterOrDigit))
				{
					buffer.Append(token);
					continue;
				}

				// get the expression without curly braces
				var tokenExpressionContent = token.Substring(1, token.Length - 2);

				// assemble the encoded version
				buffer
					.Append("{")
					.Append(encoderFunctionName)
					.Append('(')
					.Append(tokenExpressionContent)
					.Append(")}");
			}

			// return the content of the buffer
			return buffer.ToString();
		}
		#endregion
		#region Private Fields
		private static readonly PhraseScriptTokenizer Tokenizer = new PhraseScriptTokenizer();
		#endregion
	}
}