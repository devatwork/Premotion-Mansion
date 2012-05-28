using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the default block rendering behavior.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "delayedRenderingBlockBehavior")]
	public class DelayedRenderingBlockBehaviorDescriptor : DefaultBlockBehaviorDescriptor
	{
		#region Render Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		protected override void DoRender(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// get the services
			var templateService = context.Nucleus.ResolveSingle<ITemplateService>();

			// disable the output cache
			WebUtilities.DisableOutputCache(context);

			// serialize the block properties to a string
			var serializedBlockProperties = context.Nucleus.ResolveSingle<IConversionService>().Convert<string>(context, blockProperties);

			// write the function to the target field
			templateService.RenderContent(context, "{RenderBlockDelayed( '" + serializedBlockProperties + "' )}", targetField);
		}
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		public void RenderDelayed(IMansionContext context, IPropertyBag blockProperties, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (blockProperties == null)
				throw new ArgumentNullException("blockProperties");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// disable the output cache
			WebUtilities.DisableOutputCache(context);

			// perform rendering as normal
			base.DoRender(context, blockProperties, targetField);
		}
		#endregion
	}
}