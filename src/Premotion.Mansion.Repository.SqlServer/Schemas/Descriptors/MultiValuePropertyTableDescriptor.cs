using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a multi-value property table.
	/// </summary>
	[Named(typeof (TypeDescriptor), Constants.DescriptorNamespaceUri, "multiValuePropertyTable")]
	public class MultiValuePropertyTableDescriptor : PropertyTableDescriptor
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
			schema.AddOrUpdateTable(tableName, () => new MultiValuePropertyTable(tableName).Add(property.Name), table => table.Add(property.Name));
		}
		#endregion
	}
}