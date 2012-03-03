using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Implements the base tag for all <see cref="Loop"/> tags.
	/// </summary>
	public abstract class LoopBaseTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get arguments
			var targetDataspace = GetRequiredAttribute<string>(context, "target");
			var global = GetAttribute(context, "global", false);

			// get the dataset through which to loop
			var dataset = GetLoopset(context);

			// create the loop
			var start = GetAttribute(context, "start", 0);
			var end = Math.Max(start, GetAttribute(context, "end", dataset.RowCount - 1));
			var loop = new Loop(dataset, start, end);

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
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected abstract Dataset GetLoopset(MansionContext context);
	}
}