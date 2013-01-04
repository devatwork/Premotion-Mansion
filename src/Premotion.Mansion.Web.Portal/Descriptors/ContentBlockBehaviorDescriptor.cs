using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the content block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "contentBlockBehavior")]
	public class ContentBlockBehaviorDescriptor : BlockBehaviorDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ContentBlockBehaviorDescriptor(IPortalService portalService) : base(portalService)
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
			IPropertyBag contentProperties;
			if (!context.Stack.TryPeek("ContentProperties", out contentProperties))
				throw new InvalidOperationException("The ContentProperties were not found on the stack");

			// get the page type
			var pageType = contentProperties.Get<ITypeDefinition>(context, "type");

			// chech if the personalized page is set
			PersonalizedContentDescriptor personalizedContentDescriptor;
			if (pageType.TryFindDescriptorInHierarchy(out personalizedContentDescriptor))
				PortalService.RenderDelayedBlockToOutput(context, blockProperties, targetField);
			else
				PortalService.RenderBlockToOutput(context, blockProperties, targetField);
		}
		#endregion
	}
}