using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Scripts
{
	/// <summary>
	/// Breaks the execution of the current context.
	/// </summary>
	[Named(Constants.NamespaceUri, "breakExecution")]
	public class BreakExecutionTag : ScriptTag
	{
		/// <summary>
		/// Breaks the execution of the current context.
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			context.BreakExecution = true;
		}
	}
}