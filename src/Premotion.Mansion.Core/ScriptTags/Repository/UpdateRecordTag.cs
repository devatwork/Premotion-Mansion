using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Updates an existing <see cref="Record"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "updateRecord")]
	public class UpdateRecordTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var record = GetRequiredAttribute<Record>(context, "source");

			// get the properties which to edit
			var editProperties = new PropertyBag(GetAttributes(context));
			editProperties.Remove("source");
			using (context.Stack.Push("Properties", editProperties))
				ExecuteChildTags(context);

			// store the updated node
			context.Repository.Update(context, record, editProperties);
		}
	}
}