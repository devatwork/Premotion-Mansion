using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Adds a new row to an existing dataset.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "addRowToSet")]
	public class AddRowToSetTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// add the row
			GetRequiredAttribute<Dataset>(context, "target").AddRow(GetRequiredAttribute<IPropertyBag>(context, "source"));
		}
	}
}