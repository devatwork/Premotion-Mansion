using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Provides utility methods for SQL server.
	/// </summary>
	public static class SqlServerUtilities
	{
		/// <summary>
		/// Populates the full text property.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="modifiedProperties">The modified <see cref="IPropertyBag"/>.</param>
		/// <param name="originalProperties">The original <see cref="IPropertyBag"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void PopulateFullTextColumn(IMansionContext context, ITypeDefinition type, IPropertyBag modifiedProperties, IPropertyBag originalProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");
			if (originalProperties == null)
				throw new ArgumentNullException("originalProperties");

			// try to get the full text descriptor
			FullTextDescriptor descriptor;
			if (!type.TryFindDescriptorInHierarchy(out descriptor))
				return;

			// index
			descriptor.Populate(context, modifiedProperties, originalProperties);
		}
	}
}