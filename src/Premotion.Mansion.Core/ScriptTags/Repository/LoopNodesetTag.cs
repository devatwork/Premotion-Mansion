using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.ScriptTags.Stack;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Retrieves a child nodes from the repository.
	/// </summary>
	[Named(Constants.NamespaceUri, "loopNodeset")]
	public class LoopNodesetTag : LoopBaseTag
	{
		/// <summary>
		/// Gets the loop set.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="Dataset"/> on which to loop.</returns>
		protected override Dataset GetLoopset(MansionContext context)
		{
			return GetRequiredAttribute<Nodeset>(context, "source");
		}
	}
}