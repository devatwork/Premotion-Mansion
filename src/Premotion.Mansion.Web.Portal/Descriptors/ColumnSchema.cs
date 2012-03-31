using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Represents the schema of an layout.
	/// </summary>
	public class ColumnSchema
	{
		#region Constructors
		/// <summary>
		/// Constructs a column schema.
		/// </summary>
		/// <param name="columns"></param>
		public ColumnSchema(string[] columns)
		{
			// validate arguments
			if (columns == null)
				throw new ArgumentNullException("columns");

			// set values
			this.columns = columns;
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Gets the <see cref="ColumnSchema"/> of <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="typeName">The name of the type from which to get the column scheme.</param>
		/// <returns>Returns the schema.</returns>
		public static ColumnSchema GetSchema(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// get the type
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			var type = typeService.Load(context, typeName);

			return GetSchema(context, type);
		}
		/// <summary>
		/// Gets the <see cref="ColumnSchema"/> of <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="type">The <see cref="ITypeDefinition" /> of the type from which to get the column scheme.</param>
		/// <returns>Returns the <see cref="ColumnSchema"/>.</returns>
		public static ColumnSchema GetSchema(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// get the schema descriptor
			ColumnSchemaDescriptor descriptor;
			if (!type.TryGetDescriptor(out descriptor))
				throw new InvalidOperationException(string.Format("The type '{0}' does not provide a schema", type.Name));

			return descriptor.GetSchema(context);
		}
		#endregion
		#region Column Methods
		/// <summary>
		/// Checks whether a column with the name <paramref name="columnName"/> exists in this schema.
		/// </summary>
		/// <param name="columnName">The name of the column.</param>
		/// <returns>Returns true when the column exists otherwise false.</returns>
		public bool ContainsColumn(string columnName)
		{
			// validate arguments
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");

			return columns.Any(candidate => candidate.Equals(columnName, StringComparison.OrdinalIgnoreCase));
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the default column name of this schema.
		/// </summary>
		public string DefaultColumn
		{
			get
			{
				if (columns.Length == 0)
					throw new InvalidOperationException("This schema does not have a default column.");

				return columns[0];
			}
		}
		/// <summary>
		/// Gets the columns in this schema.
		/// </summary>
		public IEnumerable<string> Columns
		{
			get { return columns; }
		}
		#endregion
		#region Private Fields
		private readonly string[] columns;
		#endregion
	}
}