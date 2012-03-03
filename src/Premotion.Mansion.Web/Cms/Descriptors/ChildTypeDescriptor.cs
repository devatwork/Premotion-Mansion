using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Model;

namespace Premotion.Mansion.Web.Cms.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "childType")]
	public class ChildTypeDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public ChildTypeDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Properties
		/// <summary>
		/// Sets the <see cref="ChildType"/>s on the <paramref name="behavior"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="behavior">The <see cref="CmsBehavior"/>.</param>
		public void SetChildTypes(MansionContext context, CmsBehavior behavior)
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