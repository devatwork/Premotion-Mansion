using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the base descriptor for block behaviors.
	/// </summary>
	public abstract class BlockBehaviorDescriptor : TypeDescriptor
	{
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
	}
}