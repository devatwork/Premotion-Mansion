using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Stack;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a child nodes from the repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "loopNodeset")]
	public class LoopNodesetTag : LoopBaseTag
	{
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected override Dataset GetLoopset(IMansionContext context)
		{
			var loopset = GetRequiredAttribute<Nodeset>(context, "source");
			End = loopset.RowCount;
			return loopset;
		}
	}
}