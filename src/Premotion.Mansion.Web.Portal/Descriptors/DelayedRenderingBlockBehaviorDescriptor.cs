using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the default block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "personalizedBlockBehavior")]
	public class PersonalizedBlockBehaviorDescriptor : DefaultBlockBehaviorDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public PersonalizedBlockBehaviorDescriptor(IPortalService portalService) : base(portalService)
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
			// render the block using delayed rendering
			PortalService.RenderDelayedBlockToOutput(context, blockProperties, targetField);
		}
		#endregion
	}
}