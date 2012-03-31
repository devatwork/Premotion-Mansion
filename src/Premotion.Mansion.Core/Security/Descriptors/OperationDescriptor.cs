using System;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Security.Descriptors
{
	/// <summary>
	/// Describes the behavior of this type in the CMS.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "operation")]
	public class OperationDescriptor : TypeDescriptor
	{
		#region Methods
		/// <summary>
		/// Gets the <see cref="ProtectedOperation"/> of this operation descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The <see cref="ProtectedResource"/>.</param>
		/// <returns>Returns the <see cref="ProtectedOperation"/> instance.</returns>
		public ProtectedOperation GetOperation(IMansionContext context, ProtectedResource resource)
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