using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the page block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "pageBlockBehavior")]
	public class PageBlockBehaviorDescriptor : BlockBehaviorDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public PageBlockBehaviorDescriptor(IPortalService portalService) : base(portalService)
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
			// get the page properties
			IPropertyBag pageProperties;
			if (!context.Stack.TryPeek("PageProperties", out pageProperties))
				throw new InvalidOperationException("The PageProperties were not found on the stack");

			// get the page type
			var pageType = pageProperties.Get<ITypeDefinition>(context, "type");

			// chech if the personalized page is set
			PersonalizedPageDescriptor personalizedPageDescriptor;
			if (pageType.TryFindDescriptorInHierarchy(out personalizedPageDescriptor))
				PortalService.RenderDelayedBlockToOutput(context, blockProperties, targetField);
			else
				PortalService.RenderBlockToOutput(context, blockProperties, targetField);
		}
		#endregion
	}
}