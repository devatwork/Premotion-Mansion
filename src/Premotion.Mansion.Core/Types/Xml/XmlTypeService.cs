using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types.Xml
{
	/// <summary>
	/// Implements <see cref="IPropertyDefinition"/>
	/// </summary>
	public class XmlTypeService : ManagedLifecycleService, ITypeService, IServiceWithDependencies
	{
		#region Nested type: CachedTypeDefinition
		/// <summary>
		/// Implements <see cref="CachedObject{TObject}"/> for <see cref="ITypeDefinition"/>.
		/// </summary>
		private class CachedTypeDefinition : CachedObject<ITypeDefinition>
		{
			#region Constructors
			/// <summary>
			/// Constructs a new cached object.
			/// </summary>
			/// <param name="obj">The object which to cache.</param>
			public CachedTypeDefinition(ITypeDefinition obj) : base(obj)
			{
				Priority = Priority.NotRemovable;
			}
			#endregion
		}
		#endregion
		#region Constants
		/// <summary>
		/// The XML namespace in which the type definitions live.
		/// </summary>
		private const string Namespace = "http://schemas.premotion.nl/mansion/1.0/type.definition.xsd";
		#endregion
		#region Get Methods
		/// <summary>
		/// Loads the type with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <returns>Returns the loaded type.</returns>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified name can not be found.</exception>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public ITypeDefinition Load(IContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			ITypeDefinition type;
			if (!types.TryGetValue(typeName, out type))
				throw new TypeNotFoundException(typeName);
			return type;
		}
		/// <summary>
		/// Tries to load the type with name <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <param name="typeDefinition">The loaded type definition.</param>
		/// <returns>Returns true when the type is loaded, otherwise false.</returns>
		public bool TryLoad(IContext context, string typeName, out ITypeDefinition typeDefinition)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			return types.TryGetValue(typeName, out typeDefinition);
		}
		/// <summary>
		/// Gets the type which represents the root of the type hierarchy.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public ITypeDefinition LoadRoot(IContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return rootType;
		}
		/// <summary>
		/// Gets all the type definitions in this application.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns all the types.</returns>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public IEnumerable<ITypeDefinition> LoadAll(IContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the resource service
			return types.Values;
		}
		/// <summary>
		/// Loads the type definition for the specified type.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		/// <param name="name">The name of the type.</param>
		/// <returns>Returns the loaded type.</returns>
		private ITypeDefinition LoadTypeDefinition(INucleusAwareContext context, string name)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			var applicationResourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var resourcePath = TypeResourcePathInterpreter.TypeResourcePath.CreateTypeDefinitionResourcePath(name);
			if (!applicationResourceService.Exists(resourcePath))
				throw new TypeNotFoundException(name);

			// get the resource service
			var typeResource = applicationResourceService.GetSingle(context, resourcePath);

			// get the cache service
			var cacheKey = ResourceCacheKey.Create(typeResource);
			var cacheService = context.Nucleus.Get<ICachingService>(context);

			// create the type
			return cacheService.GetOrAdd(
				context,
				cacheKey,
				() =>
				{
					// try to create the type
					try
					{
						// load in the document
						var element = LoadElementFromDocument(context, typeResource);

						// build the namespace manager
						var nsmgr = new XmlNamespaceManager(element.OwnerDocument.NameTable);
						nsmgr.AddNamespace("xtns", Namespace);

						// create type from element
						var typeDefinition = CreateTypeFromElement(context, name, nsmgr, element);

						// create the cached type definition
						return new CachedTypeDefinition(typeDefinition);
					}
					catch (Exception ex)
					{
						throw new ParseTypeException(typeResource, ex);
					}
				});
		}
		/// <summary>
		/// Loads the element root from the resource.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="typeResource">The resource of the type definition.</param>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified path can not be found.</exception>
		private XmlElement LoadElementFromDocument(IContext context, IResource typeResource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (typeResource == null)
				throw new ArgumentNullException("typeResource");

			// read in the XML document
			var document = new XmlDocument();
			using (var resourceStream = typeResource.OpenForReading())
			using (var reader = XmlReader.Create(resourceStream.Reader, readerSettings))
				document.Load(reader);

			return document.DocumentElement;
		}
		/// <summary>
		/// Creates a type definition from the element.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		/// <param name="name">The name of the type which to load.</param>
		/// <param name="nsmgr">The namespace manager of the element.</param>
		/// <param name="element">The XML element.</param>
		private TypeDefinition CreateTypeFromElement(INucleusAwareContext context, string name, XmlNamespaceManager nsmgr, XmlNode element)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (nsmgr == null)
				throw new ArgumentNullException("nsmgr");
			if (element == null)
				throw new ArgumentNullException("element");

			// get the parent type if this type has any
			ITypeDefinition parent = null;
			var parentTypeName = element.Attributes.GetNamedItem("inherits");
			if (parentTypeName != null)
			{
				// detect self reference
				if (parentTypeName.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException(string.Format("Type '{0}' inherits from itself, please check type definition.", parentTypeName.Value));

				// load parent definition
				parent = LoadTypeDefinition(context, parentTypeName.Value);
			}
			// create the type definition
			var definition = new TypeDefinition(name, parent);

			// get the properties
			var propertyNodes = element.SelectNodes("//xtns:property", nsmgr);
			if (propertyNodes != null)
			{
				foreach (XmlNode propertyNode in propertyNodes)
				{
					// create the property
					var property = new PropertyDefinition(propertyNode.Attributes["name"].Value);
					definition.InternalProperties.Add(property);

					// get the descriptors
					var propertyDescriptorNodes = propertyNode.SelectNodes("./*", nsmgr);
					if (propertyDescriptorNodes == null)
						continue;

					foreach (XmlNode descriptorNode in propertyDescriptorNodes)
						property.AddDescriptor(LoadDescriptor(context, nsmgr, descriptorNode, definition));
				}
			}

			// get the descriptors
			var descriptorNodes = element.SelectNodes("./*[not(self::xtns:property)]", nsmgr);
			if (descriptorNodes != null)
			{
				foreach (XmlNode descriptorNode in descriptorNodes)
					definition.AddDescriptor(LoadDescriptor(context, nsmgr, descriptorNode, definition));
			}

			// return the created definition
			return definition;
		}
		/// <summary>
		/// Loads a <see cref="IDescriptor"/> from 
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		/// <param name="nsmgr">The <see cref="XmlNamespaceManager"/>.</param>
		/// <param name="descriptorNode">The <see cref="XmlNode"/> from which to load the descriptor.</param>
		/// <param name="definition">The <see cref="TypeDefinition"/>.</param>
		/// <returns>Returns the loaded <see cref="IDescriptor"/>.</returns>
		private static IDescriptor LoadDescriptor(INucleusAwareContext context, XmlNamespaceManager nsmgr, XmlNode descriptorNode, TypeDefinition definition)
		{
			// construct the descriptor from XML
			var descriptor = XmlDescriptorFactory<IDescriptor>.Create(context, descriptorNode, definition);

			// if it is a nested descriptor, process the child XML elements
			var descriptorWithNesting = descriptor as NestedTypeDescriptor;
			if (descriptorWithNesting != null)
			{
				var nestedDescriptorNodes = descriptorNode.SelectNodes("./*", nsmgr);
				if (nestedDescriptorNodes != null)
				{
					foreach (XmlNode nestedDescriptorNode in nestedDescriptorNodes)
						descriptorWithNesting.AddDescriptor(LoadDescriptor(context, nsmgr, nestedDescriptorNode, definition));
				}
			}

			return descriptor;
		}
		#endregion
		#region Overrides of ManagedLifecycleService
		/// <summary>
		/// Starts this service.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/>.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// enumerate the types
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			foreach (var typeName in resourceService.EnumeratorFolders(context, "Types"))
				types.TryAdd(typeName, LoadTypeDefinition(context, typeName));

			// find the root type
			rootType = types.Values.Single(x => !x.HasParent);
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		DependencyModel IServiceWithDependencies.Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ICachingService>().Add<IApplicationResourceService>();
		private readonly XmlReaderSettings readerSettings = new XmlReaderSettings
		                                                    {
		                                                    	ValidationType = ValidationType.Schema
		                                                    };
		private readonly ConcurrentDictionary<string, ITypeDefinition> types = new ConcurrentDictionary<string, ITypeDefinition>(StringComparer.OrdinalIgnoreCase);
		private ITypeDefinition rootType;
		#endregion
	}
}