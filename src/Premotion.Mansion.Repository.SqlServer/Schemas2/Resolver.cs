using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas2.Descriptors;

namespace Premotion.Mansion.Repository.SqlServer.Schemas2
{
	/// <summary>
	/// Implements the <see cref="Schema"/> resolver.
	/// </summary>
	public static class Resolver
	{
		#region Resolve Methods
		/// <summary>
		/// Resolves the schema for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <param name="type">The type which to resolve.</param>
		/// <returns>Returns the schema.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static Schema Resolve(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// create a new schema
			var schema = new Schema();

			// loop through all the types in the hierarchy
			foreach (var hierarchyType in type.Hierarchy)
				ExtractSchemaFromTypeDefinition(context, hierarchyType, schema);

			// make sure the schema is valid
			if (schema.RootTable == null)
				throw new InvalidOperationException(string.Format("The type '{0}' did not result in a root table", type.Name));

			return schema;
		}
		/// <summary>
		/// Extracts schema info from <paramref name="type"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="schema"></param>
		private static void ExtractSchemaFromTypeDefinition(IMansionContext context, ITypeDefinition type, Schema schema)
		{
			// check if this type has a table declaration
			TypeTableDescriptor tableDescriptor;
			if (type.TryGetDescriptor(out tableDescriptor))
			{
				var table = tableDescriptor.Create(context, type);

				// add the table to schema
				schema.Add(table);
			}

			// check if the type has property table declaration
			foreach (var property in type.Properties)
			{
				PropertyTableDescriptor propertyTableDescriptor;
				if (!property.TryGetDescriptor(out propertyTableDescriptor))
					continue;

				// create the table
				propertyTableDescriptor.Create(context, schema, property);
			}
		}
		#endregion
	}
}