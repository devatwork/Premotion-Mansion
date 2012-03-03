using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a property table.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "singleValuePropertyTable")]
	public class SingleValuePropertyTableDescriptor : PropertyTableDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public SingleValuePropertyTableDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates the property table as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <param name="propertyName"></param>
		/// <param name="isOwner"></param>
		/// <returns></returns>
		public override void CreateTableInSchema(MansionContext context, Schema schema, string propertyName, bool isOwner)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// create or update the table
			var tableName = Properties.Get<string>(context, "tableName");
			schema.AddOrUpdateTable(tableName, () => new SingleValuePropertyTable(tableName, propertyName), table => { throw new InvalidOperationException("Single value property tables can only be used once"); }, isOwner);
		}
		#endregion
	}
}