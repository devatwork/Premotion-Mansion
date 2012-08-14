using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2.Descriptors
{
	/// <summary>
	/// Describes a type table.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "typeTable")]
	public class TypeTableDescriptor : TypeDescriptor
	{
		#region Overrides of TableDescriptor
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Table"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public Table Create(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// invoke template method
			return DoCreate(context, type);
		}
		/// <summary>
		/// Creates the <see cref="Table"/> from the descriptor.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <returns>Returns the created <see cref="Table"/>.</returns>
		protected virtual Table DoCreate(IMansionContext context, ITypeDefinition type)
		{
			// get the name of the table
			var tableName = Properties.Get<string>(context, "tableName");

			// create the type table
			var table = new TypeTable(tableName);

			// create the columns for this table
			foreach (var property in type.Properties)
			{
				ColumnDescriptor columnDescriptor;
				if (!property.TryGetDescriptor(out columnDescriptor))
					continue;

				// add the column to the table
				table.Add(columnDescriptor.Create(context, table, property));
			}

			// return the created table
			return table;
		}
		#endregion
	}
}