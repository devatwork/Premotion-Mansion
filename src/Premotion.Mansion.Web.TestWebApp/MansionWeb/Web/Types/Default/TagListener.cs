using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Listeners;

namespace Premotion.Mansion.Web.Web.Types.Default
{
	/// <summary>
	/// This listener manages tags.
	/// </summary>
	public class TagListener : NodeListener
	{
		#region Overrides of NodeListener
		/// <summary>
		/// This method is called just before a node is created by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parent">The parent node to which the new child will be added.</param>
		/// <param name="newProperties">The new properties of the node.</param>
		protected override void DoBeforeCreate(IMansionContext context, Node parent, IPropertyBag newProperties)
		{
			TagUtilities.ToGuids(context, newProperties);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The node which will be modified.</param>
		/// <param name="modifiedProperties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(IMansionContext context, Node node, IPropertyBag modifiedProperties)
		{
			TagUtilities.ToGuids(context, modifiedProperties);
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="node">The <see cref="Node"/> which does not have the property..</param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		protected override bool DoTryResolveMissingProperty(IMansionContext context, Node node, string propertyName, out object value)
		{
			// we do not care about any property except _tags
			if (!"_tags".Equals(propertyName, StringComparison.OrdinalIgnoreCase))
			{
				value = null;
				return false;
			}

			// initialize the _tags attribute
			value = TagUtilities.ToNames(context, node);
			return true;
		}
		#endregion
	}
}