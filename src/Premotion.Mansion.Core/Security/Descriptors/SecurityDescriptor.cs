using System;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Security.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "security")]
	public class SecurityDescriptor : NestedTypeDescriptor
	{
		#region Methods
		/// <summary>
		/// Gets the <see cref="ProtectedResource"/> of this security descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="ProtectedResource"/> instance.</returns>
		public ProtectedResource GetResource(IMansionContext context)
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