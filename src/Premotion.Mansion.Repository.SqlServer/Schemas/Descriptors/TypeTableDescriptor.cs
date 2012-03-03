using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a type table.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "typeTable")]
	public class TypeTableDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public TypeTableDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates the table as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="schema"></param>
		/// <returns></returns>
		public virtual Table Create(MansionContext context, Schema schema)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (schema == null)
				throw new ArgumentNullException("schema");

			// create the type table
			var tableName = Properties.Get<string>(context, "tableName");
			return new TypeTable(tableName);
		}
		#endregion
	}
}