using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Loops through all the rows in a dataset.
	/// </summary>
	[Named(Constants.NamespaceUri, "loopDataset")]
	public class LoopDatasetTag : LoopBaseTag
	{
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected override Dataset GetLoopset(MansionContext context)
		{
			return GetRequiredAttribute<Dataset>(context, "source");
		}
	}
}