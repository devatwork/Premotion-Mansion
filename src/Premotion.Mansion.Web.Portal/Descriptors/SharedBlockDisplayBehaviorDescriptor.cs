using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the behavior for shared blocks displays.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "sharedBlockDisplayBehavior")]
	public class SharedBlockDisplayBehaviorDescriptor : BlockBehaviorDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService">The <see cref="IPortalService"/>.</param>
		public SharedBlockDisplayBehaviorDescriptor(IPortalService portalService) : base(portalService)
		{
		}
		#endregion
		#region Overrides of BlockBehaviorDescriptor
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		protected override void DoRender(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// first retrieve the block node to display
			Guid displayedBlockGuid;
			if (!blockProperties.TryGet(context, "blockGuid", out displayedBlockGuid))
				throw new InvalidOperationException("Block guid not found for shared block display");
			var displayedBlockNode = context.Repository.RetrieveSingleNode(context, new PropertyBag
			                                                                        {
			                                                                        	{"guid", displayedBlockGuid}
			                                                                        });
			if (displayedBlockNode == null)
				throw new InvalidOperationException(string.Format("Could not find block with guid '{0}'", displayedBlockGuid));

			// second, merge the two block properties together
			var mergedBlockProperties = new PropertyBag();
			mergedBlockProperties.Merge(blockProperties);
			mergedBlockProperties.Merge(displayedBlockNode);
			mergedBlockProperties.Set("id", blockProperties.Get<int>(context, "id"));

			// finally re-render the combined block using the portal service
			PortalService.RenderBlock(context, mergedBlockProperties, targetField);
		}
		#endregion
	}
}