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
		#region Protected Fields
		/// <summary>
		/// Start of the loop
		/// </summary>
		protected int Start = 0;

		/// <summary>
		/// End of the loop
		/// </summary>
		protected int End = 0;
		#endregion

		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var targetDataspace = GetRequiredAttribute<string>(context, "target");
			var global = GetAttribute(context, "global", false);

			// get the dataset through which to loop
			var dataset = GetLoopset(context);

			// create the loop
			var loop = new Loop(new DatasetReader(dataset), Start, End);

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
		protected abstract Dataset GetLoopset(IMansionContext context);
	}
}