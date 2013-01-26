using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Loops over a <see cref="IPropertyBagReader"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "loopReader")]
	public class LoopReaderTag : ScriptTag
	{
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var targetDataspace = GetRequiredAttribute<string>(context, "target");
			var global = GetAttribute(context, "global", false);

			// create the loop
			var start = GetAttribute(context, "start", 0);
			var end = Math.Max(start, GetAttribute(context, "end", int.MaxValue));
			var loop = new Loop(context.Reader, start, end);

			// push the loop to the stack
			using (context.Stack.Push("Loop", loop))
			{
				// loop through all the rows
				foreach (var row in loop.Rows)
				{
					// push the row to the stack
					using (context.Stack.Push(targetDataspace, row, global))
						ExecuteChildTags(context);
				}
			}
		}
	}
}