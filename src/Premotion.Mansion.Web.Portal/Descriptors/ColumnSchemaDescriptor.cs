using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the descriptor for layouts.
	/// </summary>
	[Named(Constants.DescriptorNamespaceUri, "columnSchema")]
	public class ColumnSchemaDescriptor : TypeDescriptor
	{
		#region Constructors
		/// <summary>
		/// </summary>
		/// <param name="namespaceUri">The namespace.</param>
		/// <param name="name">The name of this descriptor.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> to which this descriptor is applied.</param>
		public ColumnSchemaDescriptor(string namespaceUri, string name, IPropertyBag properties, ITypeDefinition typeDefinition) : base(namespaceUri, name, properties, typeDefinition)
		{
		}
		#endregion
		#region Schema Methods
		/// <summary>
		/// Gets the <see cref="ColumnSchema"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="ColumnSchema"/>.</returns>
		public ColumnSchema GetSchema(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the columns
			var columns = Properties.Get<string>(context, "columns");

			return new ColumnSchema(columns.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
		}
		#endregion
	}
}