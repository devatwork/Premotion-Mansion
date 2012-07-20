using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Executes the nested tags of the current <see cref="InvokeProcedureTag"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "executeNestedTags")]
	public class ExecuteNestedTagsTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// peek the top most procedure from the callstack
			ScriptTag procedure;
			if (!context.ProcedureCallStack.TryPeek(out procedure))
				throw new InvalidOperationException("The invokeNestedTags tag can only be used within the context of a procedure");

			// determine which tag to execute
			procedure.ExecuteChildTags(context);
		}
	}
}