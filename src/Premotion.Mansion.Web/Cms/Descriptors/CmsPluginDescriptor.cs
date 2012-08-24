using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Cms.Descriptors
{
	/// <summary>
	/// Describes a CMS plugin.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "plugin")]
	public class CmsPluginDescriptor : TypeDescriptor
	{
		#region Properties
		/// <summary>
		/// Gets the order of this plugin.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the order number.</returns>
		public int GetOrder(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return Properties.Get<int>(context, "order");
		}
		#endregion
	}
}