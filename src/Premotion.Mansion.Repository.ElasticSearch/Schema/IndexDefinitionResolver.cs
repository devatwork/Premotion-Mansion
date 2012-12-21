using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.ElasticSearch.Schema.Descriptors;

namespace Premotion.Mansion.Repository.ElasticSearch.Schema
{
	/// <summary>
	/// Resolves <see cref="IndexDefinition"/>s from <see cref="ITypeDefinition"/>.
	/// </summary>
	public class IndexDefinitionResolver
	{
		#region Constructors
		/// <summary>
		/// Constructs the index definition resolver.
		/// </summary>
		/// <param name="typeService">The <see cref="ITypeDefinition"/>.</param>
		public IndexDefinitionResolver(ITypeService typeService)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Resolve Methods
		/// <summary>
		/// Resolves all the <see cref="IndexDefinition"/>s defined by the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="IndexDefinition"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public IEnumerable<IndexDefinition> ResolveAll(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// loop over all the type which directly have an index definition
			foreach (var type in typeService.LoadAll(context))
			{
				IndexDescriptor descriptor;
				if (!type.TryGetDescriptor(out descriptor))
					continue;

				// create and return the index definition
				yield return CreateDefinitionFromDescriptor(context, descriptor);
			}
		}
		/// <summary>
		/// Resolves the <see cref="IndexDefinition"/>s defined for the given <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the type for which to get the index definitions.</param>
		/// <returns>Returns the <see cref="IndexDefinition"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public IEnumerable<IndexDefinition> Resolve(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// load the type
			var type = typeService.Load(context, typeName);

			// find all the index definitions 
			return Resolve(context, type);
		}
		/// <summary>
		/// Resolves the <see cref="IndexDefinition"/>s defined for the given <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <returns>Returns the <see cref="IndexDefinition"/>s.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public IEnumerable<IndexDefinition> Resolve(IMansionContext context, ITypeDefinition type)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (type == null)
				throw new ArgumentNullException("type");

			// find all the index definitions 
			return type.GetDescriptorsInHierarchy<IndexDescriptor>().Select(descriptor => CreateDefinitionFromDescriptor(context, descriptor));
		}
		/// <summary>
		/// Creates a <see cref="IndexDefinition"/> from the given <paramref name="descriptor"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="descriptor"></param>
		/// <returns></returns>
		private static IndexDefinition CreateDefinitionFromDescriptor(IMansionContext context, IndexDescriptor descriptor)
		{
			// create index definition
			var definition = descriptor.CreateDefinition(context);

			// create type mapping for this type and its children
			CreateTypeMapping(context, definition, descriptor.TypeDefinition);

			// return the definition
			return definition;
		}
		#endregion
		#region Type Mapping Methods
		/// <summary>
		/// Creates the type mapping for the given <paramref name="type"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="IndexDefinition"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		private static void CreateTypeMapping(IMansionContext context, IndexDefinition definition, ITypeDefinition type)
		{
			// create the type mapping
			var typeMapping = new TypeMapping(type);

			// map the type
			MapType(context, definition, type, typeMapping);
		}
		/// <summary>
		/// Maps the given <paramref name="type"/> in <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="definition">The <see cref="IndexDefinition"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/>.</param>
		private static void MapType(IMansionContext context, IndexDefinition definition, ITypeDefinition type, TypeMapping typeMapping)
		{
			// map the type descriptor, if any
			TypeMappingDescriptor descriptor;
			if (type.TryGetDescriptor(out descriptor))
				descriptor.UpdateMapping(context, typeMapping);

			// map all the properties of this type
			MapProperties(context, type, typeMapping);

			// loop over all the children of this type
			foreach (var childType in type.GetChildTypes(context))
			{
				// clone the parent mapping, children should include the properties of the parent
				var childMapping = typeMapping.Clone(childType);

				// map the the child
				MapType(context, definition, childType, childMapping);
			}

			// append the mapping to the index definition
			definition.Add(typeMapping);
		}
		/// <summary>
		/// Maps the properties of the given <paramref name="type"/> in <paramref name="typeMapping"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="type">The <see cref="ITypeDefinition"/>.</param>
		/// <param name="typeMapping">The <see cref="TypeMapping"/>.</param>
		private static void MapProperties(IMansionContext context, ITypeDefinition type, TypeMapping typeMapping)
		{
			// loop over all the properties to find those with descriptors
			foreach (var property in type.Properties)
			{
				// try to get the property descriptor
				PropertyMappingBaseDescriptor descriptor;
				if (!property.TryGetDescriptor(out descriptor))
					continue;

				// allow the descriptor to add property mappings to the type mapping
				descriptor.AddMappingTo(context, property, typeMapping);
			}
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}