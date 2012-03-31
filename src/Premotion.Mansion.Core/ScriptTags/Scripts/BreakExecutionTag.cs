using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Scripts
{
	/// <summary>
	/// Breaks the execution of the current context.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "breakExecution")]
	public class BreakExecutionTag : ScriptTag
	{
		/// <summary>
		/// Breaks the execution of the current context.
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			context.BreakExecution = true;
		}
	}
}