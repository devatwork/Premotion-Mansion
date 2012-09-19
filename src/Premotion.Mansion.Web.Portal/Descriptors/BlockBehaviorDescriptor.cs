using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the base descriptor for block behaviors.
	/// </summary>
	public abstract class BlockBehaviorDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		protected BlockBehaviorDescriptor(IPortalService portalService)
		{
			// validate arguments
			if (portalService == null)
				throw new ArgumentNullException("portalService");

			// set values
			this.portalService = portalService;
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		public void Render(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (blockProperties == null)
				throw new ArgumentNullException("blockProperties");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// invoke template method
			DoRender(context, blockProperties, targetField);
		}
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		protected abstract void DoRender(IMansionContext context, IPropertyBag blockProperties, string targetField);
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="IPortalService"/>.
		/// </summary>
		protected IPortalService PortalService
		{
			get { return portalService; }
		}
		#endregion
		#region Private Fields
		private readonly IPortalService portalService;
		#endregion
	}
}