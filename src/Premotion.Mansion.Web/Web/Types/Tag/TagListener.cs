using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.Web.Web.Types.Tag
{
	/// <summary>
	/// This listener manages the behavior of tags.
	/// </summary>
	public class TagListener : NodeListener
	{
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		protected override void DoBeforeCreate(MansionContext context, Node parent, IPropertyBag newProperties)
		{
			// make sure the name is normalized
			newProperties.Set("name", TagUtilities.Normalize(newProperties.Get(context, "name", string.Empty)));
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(MansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			// if the name has not changed we are not interested
			string newName;
			if (!modifiedProperties.TryGet(context, "name", out newName))
				return;

			// if the name has not changed after normalization we are not interested
			newName = TagUtilities.Normalize(newName);
			if (node.Pointer.Name.Equals(newName))
			{
				modifiedProperties.Remove("name");
				return;
			}
			modifiedProperties.Set("name", newName);

			// if the tag is renamed to another already existing tag, move all content to that existing tag and delete this one
			Node existingTag;
			var tagIndexNode = TagUtilities.RetrieveTagIndexNode(context);
			if (TagUtilities.TryRetrieveTagNode(context, tagIndexNode, newName, out existingTag))
			{
				// TODO: move all content to the existing tag

				// TODO: delete this tag
			}
		}
	}
}