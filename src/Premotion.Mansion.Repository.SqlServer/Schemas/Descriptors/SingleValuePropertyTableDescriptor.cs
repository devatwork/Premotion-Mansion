using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a property table.
	/// </summary>
	[Named(typeof (TypeDescriptor), Constants.DescriptorNamespaceUri, "singleValuePropertyTable")]
	public class SingleValuePropertyTableDescriptor : PropertyTableDescriptor
	{
		#region Overrides of PropertyTableDescriptor
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="schema">The <see cref="Schema"/>in which to create the table.</param>
		/// <param name="property">The <see cref="IPropertyDefinition"/>.</param>
		protected override void DoCreate(IMansionContext context, Schema schema, IPropertyDefinition property)
		{
			var tableName = Properties.Get<string>(context, "tableName");
			schema.AddOrUpdateTable(tableName, () => new SingleValuePropertyTable(tableName, property.Name), table => { throw new InvalidOperationException(string.Format("Single value property tables can only be used once, see {0}.{1}", TypeDefinition.Name, property.Name)); });
		}
		#endregion
	}
}