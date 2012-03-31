using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Model;

namespace Premotion.Mansion.Web.Cms.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "childType")]
	public class ChildTypeDescriptor : TypeDescriptor
	{
		#region Properties
		/// <summary>
		/// Sets the <see cref="ChildType"/>s on the <paramref name="behavior"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="behavior">The <see cref="CmsBehavior"/>.</param>
		public void SetChildTypes(IMansionContext context, CmsBehavior behavior)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (behavior == null)
				throw new ArgumentNullException("behavior");

			// return the child type
			ChildType.Create(context, this, behavior);
		}
		#endregion
	}
}