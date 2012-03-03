using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Copies a node in the top most repository.
	/// </summary>
	[Named(Constants.NamespaceUri, "copyNode")]
	public class CopyNodeTag : ScriptTag
	{
		/// <summary>
		/// Copies a node in the top most repository.
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the dataspaces
			var source = GetRequiredAttribute<Node>(context, "source");
			var newParent = GetRequiredAttribute<Node>(context, "newParent");

			// copy the node
			var modifiedNode = context.Repository.Copy(context, source.Pointer, newParent.Pointer);

			// push the node to the stack
			using (context.Stack.Push(GetRequiredAttribute<string>(context, "target"), modifiedNode, GetAttribute(context, "global", false)))
				ExecuteChildTags(context);
		}
	}
}