using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Removes an existing row from an existing dataset.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "removeRowFromSet")]
	public class RemoveRowFromSetTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// add the row
			GetRequiredAttribute<Dataset>(context, "target").RemoveRow(GetRequiredAttribute<IPropertyBag>(context, "source"));
		}
	}
}