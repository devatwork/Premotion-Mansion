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
		/// <param name="properties">The new properties of the node.</param>
		protected override void DoBeforeCreate(IMansionContext context, IPropertyBag properties)
		{
			TagUtilities.ToGuids(context, properties);
		}
		/// <summary>
		/// This method is called just before a node is updated by the repository.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="properties">The updated properties of the node.</param>
		protected override void DoBeforeUpdate(IMansionContext context, Record record, IPropertyBag properties)
		{
			TagUtilities.ToGuids(context, properties);
		}
		/// <summary>
		/// This method is called when an property which is not on the node is accessed. Useful for lazy loading properties.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="record"> </param>
		/// <param name="propertyName">The name of the property being retrieved.</param>
		/// <param name="value">The missing value</param>
		protected override bool DoTryResolveMissingProperty(IMansionContext context, Record record, string propertyName, out object value)
		{
			// we do not care about any property except _tags
			if (!"_tags".Equals(propertyName, StringComparison.OrdinalIgnoreCase))
			{
				value = null;
				return false;
			}

			// initialize the _tags attribute
			value = TagUtilities.ToNames(context, record);
			return true;
		}
		#endregion
	}
}