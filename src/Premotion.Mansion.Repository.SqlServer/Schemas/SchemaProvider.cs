using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Descriptors;

namespace Premotion.Mansion.Repository.SqlServer.Schemas
{
	/// <summary>
	/// Provides a schema for the specified type.
	/// </summary>
	public class SchemaProvider
	{
		#region Resolve Methods
		/// <summary>
		/// Resolves the schema for the specified <paramref name="query"/>.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <param name="query">The type.</param>
		/// <returns>Returns the </returns>
		public static Schema Resolve(IMansionContext context, NodeQuery query)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (query == null)
				throw new ArgumentNullException("query");

			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// check if there is a type specification
			var definedTypes = query.Clauses.Where(candidate => candidate is ITypeSpecifierClause).SelectMany(candidate => ((ITypeSpecifierClause) candidate).Types).ToArray();
			var type = definedTypes.Length > 0 ? definedTypes.First().HierarchyReverse.Where(candidate => definedTypes.All(x => x.IsAssignable(candidate))).FirstOrDefault() : typeService.LoadRoot(context);

			// resolve
			return Resolve(context, type);
		}
		/// <summary>
		/// Resolves the schema for the specified <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <param name="typeName">The name of the type type.</param>
		/// <returns>Returns the schema.</returns>
		public static Schema Resolve(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// get the type
			ITypeDefinition type;
			if (!typeService.TryLoad(context, typeName, out type))
				type = typeService.LoadRoot(context);

			// resolve)
			return Resolve(context, type);
		}
		/// <summary>
		/// Resolves the schema for the specified <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The application context.</param>
		/// <param name="type">The type which to resolve.</param>
		/// <returns>Returns the schema.</returns>
		public static Schema Resolve(IMansionContext context, ITypeDefinition type)
		{
			// create a new schema
			var schema = new Schema();

			// extract info from this type
			ExtractSchemaFromTypeDefinition(context, type, schema, true);

			// loop through all the types in the hierarchy
			if (type.HasParent)
			{
				foreach (var hierarchyType in type.Parent.HierarchyReverse)
					ExtractSchemaFromTypeDefinition(context, hierarchyType, schema, false);
			}

			return schema;
		}
		/// <summary>
		/// Extracts schema info from <paramref name="type"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="schema"></param>
		/// <param name="isOwner"></param>
		private static void ExtractSchemaFromTypeDefinition(IMansionContext context, ITypeDefinition type, Schema schema, bool isOwner)
		{
			// check if this type has a table declaration
			TypeTableDescriptor tableDescriptor;
			if (type.TryGetDescriptor(out tableDescriptor))
			{
				var table = tableDescriptor.Create(context, schema);

				// add the table to schema
				schema.AddTable(table, isOwner);

				// create the columns for this table
				foreach (var property in type.Properties)
				{
					ColumnDescriptor columnDescriptor;
					if (!property.TryGetDescriptor(out columnDescriptor))
						continue;

					// add the column to the table
					table.AddColumn(columnDescriptor.Create(context, property.Name));
				}
			}

			// check if the type has property table declaration
			foreach (var property in type.Properties)
			{
				PropertyTableDescriptor propertyTableDescriptor;
				if (!property.TryGetDescriptor(out propertyTableDescriptor))
					continue;

				// create the table
				propertyTableDescriptor.CreateTableInSchema(context, schema, property.Name, isOwner);
			}
		}
		#endregion
	}
}