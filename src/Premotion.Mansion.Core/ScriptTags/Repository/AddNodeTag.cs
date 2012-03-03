using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Repository
{
	/// <summary>
	/// Creates a new node in the topmost repository.
	/// </summary>
	[Named(Constants.NamespaceUri, "addNode")]
	public class AddNodeTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get arguments
			var parentSource = GetRequiredAttribute<Node>(context, "parentSource");

			// get the properties which to edit
			var newProperties = new PropertyBag(GetAttributes(context));
			newProperties.Remove("parentSource");
			newProperties.Remove("target");
			newProperties.Remove("global");
			using (context.Stack.Push("NewProperties", newProperties, false))
				ExecuteChildTags(context);

			// store the updated node
			var createdNode = context.Repository.Create(context, parentSource, newProperties);

			// push the new node to the stack
			context.Stack.Push(GetRequiredAttribute<string>(context, "target"), createdNode, GetAttribute<bool>(context, "global")).Dispose();
		}
	}
}