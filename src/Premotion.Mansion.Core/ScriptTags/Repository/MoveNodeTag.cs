using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Moves a node in the top most repository.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "moveNode")]
	public class MoveNodeTag : ScriptTag
	{
		/// <summary>
		/// Moves a node in the top most repository.
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the dataspaces
			var source = GetRequiredAttribute<Node>(context, "source");
			var newParent = GetRequiredAttribute<Node>(context, "newParent");

			// move the node
			var modifiedNode = context.Repository.MoveNode(context, source.Pointer, newParent.Pointer);

			// push the node to the stack
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), modifiedNode, GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
	}
}