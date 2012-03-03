using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Represents the base class for all property tables.
	/// </summary>
	public abstract class PropertyTableDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public PropertyTableDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
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
		public abstract void CreateTableInSchema(MansionContext context, Schema schema, string propertyName, bool isOwner);
		#endregion
	}
}