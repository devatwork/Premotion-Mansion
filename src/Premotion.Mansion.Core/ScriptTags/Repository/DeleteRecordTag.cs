using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Deletes a <see cref="Record"/> from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "deleteRecord")]
	public class DeleteRecordTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var record = GetRequiredAttribute<Record>(context, "source");

			// delete the node
			context.Repository.Delete(context, record);
		}
	}
}