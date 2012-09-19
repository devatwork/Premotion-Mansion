using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the default block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "defaultBlockBehavior")]
	public class DefaultBlockBehaviorDescriptor : BlockBehaviorDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DefaultBlockBehaviorDescriptor(IPortalService portalService) : base(portalService)
		{
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		protected override void DoRender(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			PortalService.RenderBlockToOutput(context, blockProperties, targetField);
		}
		#endregion
	}
}