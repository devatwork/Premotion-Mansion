using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags
{
	/// <summary>
	/// Loops over a specific range.
	/// </summary>
	[Named(Constants.NamespaceUri, "loop")]
	public class LoopTag : LoopBaseTag
	{
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected override Dataset GetLoopset(MansionContext context)
		{
			// get the range
			var start = GetRequiredAttribute<int>(context, "start");
			var end = GetRequiredAttribute<int>(context, "end");

			// create the dataset
			var dataset = new Dataset();

			// fill the dataset
			for (var index = start; index <= end; index++)
			{
				dataset.AddRow(new PropertyBag
				               {
				               	{"index", index}
				               });
			}

			return dataset;
		}
	}
}