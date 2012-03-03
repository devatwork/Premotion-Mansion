using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes a column.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "booleanColumn")]
	public class BooleanColumnDescriptor : ColumnDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public BooleanColumnDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Creates the column as described.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public override Column Create(MansionContext context, string propertyName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// get the column name
			var columnName = Properties.Get<string>(context, "columnName", null) ?? propertyName;

			// create the column
			var column = new BooleanPropertyColumn(propertyName, columnName, Properties);
			column.Initialize(context);
			return column;
		}
		#endregion
	}
}