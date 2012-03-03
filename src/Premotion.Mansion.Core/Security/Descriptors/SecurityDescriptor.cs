using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Security.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "security")]
	public class SecurityDescriptor : NestedTypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public SecurityDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Methods
		/// <summary>
		/// Gets the <see cref="ProtectedResource"/> of this security descriptor.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the <see cref="ProtectedResource"/> instance.</returns>
		public ProtectedResource GetResource(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// create the resource
			var resource = new ProtectedResource(TypeDefinition.Name);

			// loop over all the operation
			foreach (var operationDescriptor in GetDescriptors<OperationDescriptor>())
			{
				// create the operation
				var operation = operationDescriptor.GetOperation(context, resource);

				// add the operation to the resource
				resource.Add(operation);
			}

			return resource;
		}
		#endregion
	}
}