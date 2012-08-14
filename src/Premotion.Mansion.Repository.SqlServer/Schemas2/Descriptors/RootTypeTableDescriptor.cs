using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2.Descriptors
{
	/// <summary>
	/// Describes the root type table.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "rootTypeTable")]
	public class RootTypeTableDescriptor : TypeTableDescriptor
	{
		#region Factory Methods
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Table"/>.</returns>
		protected override Table DoCreate(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// create the type table
			var tableName = Properties.Get<string>(context, "tableName");
			return new RootTable(tableName);
		}
		#endregion
	}
}