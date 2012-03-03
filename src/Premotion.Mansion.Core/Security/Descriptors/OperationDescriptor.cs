using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Security.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "operation")]
	public class OperationDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public OperationDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Methods
		/// <summary>
		/// Gets the <see cref="ProtectedOperation"/> of this operation descriptor.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resource">The <see cref="ProtectedResource"/>.</param>
		/// <returns>Returns the <see cref="ProtectedOperation"/> instance.</returns>
		public ProtectedOperation GetOperation(MansionContext context, ProtectedResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// create the operation
			return new ProtectedOperation(Properties.Get<string>(context, "id"), resource)
			       {
			       	Name = Properties.Get(context, "name", Properties.Get<string>(context, "id"))
			       };
		}
		#endregion
	}
}