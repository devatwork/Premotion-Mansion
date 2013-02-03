using System;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Patterns.Descriptors;
using Premotion.Mansion.Core.Patterns.Voting;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.Templating.Html
{
	/// <summary>
	/// Implements the interpreter for html sections.
	/// </summary>
	public class HtmlSectionInterpreter : SectionInterpreter
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
			return subject.StartsWith(SectionStart, StringComparison.OrdinalIgnoreCase) && subject.EndsWith(SectionEnd, StringComparison.OrdinalIgnoreCase) ? VoteResult.MediumInterest : VoteResult.Refrain;
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

			// parse the content
			var expressionScriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
			var expression = expressionScriptService.Parse(context, new LiteralResource(content.Trim()));

			// create the section
			return new Section(context, headerDescriptor.Properties, expression);
		}
		#endregion
	}
}