using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Loops through all the rows in a dataset.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "loopDataset")]
	public class LoopDatasetTag : LoopBaseTag
	{
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected override Dataset GetLoopset(IMansionContext context)
		{
			var loopset = GetRequiredAttribute<Dataset>(context, "source");
			End = loopset.RowCount;
			return loopset;
		}
	}
}