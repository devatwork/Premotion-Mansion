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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The new properties of the node.</param>
		protected override void DoBeforeCreate(IMansionContext context, IPropertyBag properties)
		{
			// make sure the name is normalized
			properties.Set("name", TagUtilities.Normalize(properties.Get(context, "name", string.Empty)));
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			// if the name has not changed we are not interested
			string newName;
			if (!properties.TryGet(context, "name", out newName))
				return;

			// if the name has not changed after normalization we are not interested
			newName = TagUtilities.Normalize(newName);
			if (record.Get(context, "name", string.Empty).Equals(newName))
			{
				properties.Remove("name");
				return;
			}
			properties.Set("name", newName);

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