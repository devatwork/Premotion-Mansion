using System;
using System.Linq;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Boolean
{
	/// <summary>
	/// Implements the switch condition tag.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "switch")]
	public class SwitchTag : ScriptTag
	{
		/// <summary>
		/// Executes the tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the condition
			var condition = GetAttribute(context, "condition", string.Empty) ?? string.Empty;

			// check for each case
			foreach (var caseTag in GetAlternativeChildren<SwitchCaseTag>().Where(caseTag => condition.Equals(caseTag.GetAttribute<string>(context, "value"), StringComparison.OrdinalIgnoreCase)))
			{
				// execute this case
				caseTag.Execute(context);
				return;
			}

			// check if there is a default case
			DefaultCaseTag defaultCase;
			if (TryGetAlternativeChildTag(out defaultCase))
				defaultCase.Execute(context);
		}
	}
}