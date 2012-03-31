using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Deletes a node from the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "deleteNode")]
	public class DeleteNodeTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get arguments
			var source = GetRequiredAttribute<Node>(context, "source");

			// delete the node
			context.Repository.Delete(context, source.Pointer);
		}
	}
}