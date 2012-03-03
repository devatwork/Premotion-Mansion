using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Stack
{
	/// <summary>
	/// Adds a new row to an existing dataset.
	/// </summary>
	[Named(Constants.NamespaceUri, "addRowToSet")]
	public class AddRowToSetTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// add the row
			GetRequiredAttribute<Dataset>(context, "target").AddRow(GetRequiredAttribute<IPropertyBag>(context, "source"));
		}
	}
}