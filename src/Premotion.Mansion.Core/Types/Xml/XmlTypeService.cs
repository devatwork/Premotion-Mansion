using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns.Descriptors;

namespace Premotion.Mansion.Core.Types.Xml
{
	/// <summary>
	/// Implements <see cref="IPropertyDefinition"/>
	/// </summary>
	public class XmlTypeService : ITypeService
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
		#region Constructors
		/// <summary>
		/// Constructs the XML Type service.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="applicationResourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="cachingService"/> or <paramref name="applicationResourceService"/> is null.</exception>
		public XmlTypeService(ICachingService cachingService, IApplicationResourceService applicationResourceService)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");

			// set values
			this.cachingService = cachingService;
			this.applicationResourceService = applicationResourceService;
		}
		#endregion
		#region Get Methods
		/// <summary>
		/// Loads the type with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <returns>Returns the loaded type.</returns>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified name can not be found.</exception>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public ITypeDefinition Load(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// make sure the type service is initialized
			Initialize(context);

			ITypeDefinition type;
			if (!types.TryGetValue(typeName, out type))
				type = rootType;
			return type;
		}
		/// <summary>
		/// Tries to load the type with name <paramref name="typeName"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the type which to load.</param>
		/// <param name="typeDefinition">The loaded type definition.</param>
		/// <returns>Returns true when the type is loaded, otherwise false.</returns>
		public bool TryLoad(IMansionContext context, string typeName, out ITypeDefinition typeDefinition)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			// make sure the type service is initialized
			Initialize(context);

			return types.TryGetValue(typeName, out typeDefinition);
		}
		/// <summary>
		/// Gets the type which represents the root of the type hierarchy.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public ITypeDefinition LoadRoot(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// make sure the type service is initialized
			Initialize(context);

			return rootType;
		}
		/// <summary>
		/// Gets all the type definitions in this application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns all the types.</returns>
		/// <exception cref="ParseTypeException">Thrown when a type can not be parsed.</exception>
		public IEnumerable<ITypeDefinition> LoadAll(IMansionContext context)
		{
			// make sure the type service is initialized
			Initialize(context);

			return types.Values;
		}
		/// <summary>
		/// Initializes this service.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		private void Initialize(IMansionContext context)
		{
			// make thread safe
			if ((initializedState != 0) || (Interlocked.CompareExchange(ref initializedState, 1, 0) != 0))
				return;

			// enumerate the types
			types = applicationResourceService.EnumeratorFolders(context, "Types").Select(typeName => new
			                                                                                          {
			                                                                                          	Name = typeName,
			                                                                                          	Definition = LoadTypeDefinition(context, typeName)
			                                                                                          }).ToDictionary(x => x.Name, x => x.Definition, StringComparer.OrdinalIgnoreCase);

			// find the root type
			rootType = types.Values.Single(x => !x.HasParent);
		}
		/// <summary>
		/// Loads the type definition for the specified type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="name">The name of the type.</param>
		/// <returns>Returns the loaded type.</returns>
		private ITypeDefinition LoadTypeDefinition(IMansionContext context, string name)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			var resourcePath = TypeResourcePathInterpreter.TypeResourcePath.CreateTypeDefinitionResourcePath(name);
			if (!applicationResourceService.Exists(context, resourcePath))
				throw new TypeNotFoundException(name);

			// get the resource service
			var typeResource = applicationResourceService.GetSingle(context, resourcePath);

			// create the type
			var cacheKey = ResourceCacheKey.Create(typeResource);
			return cachingService.GetOrAdd(
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeResource">The resource of the type definition.</param>
		/// <exception cref="TypeNotFoundException">Thrown when the type with the specified path can not be found.</exception>
		private XmlElement LoadElementFromDocument(IMansionContext context, IResource typeResource)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="name">The name of the type which to load.</param>
		/// <param name="nsmgr">The namespace manager of the element.</param>
		/// <param name="element">The XML element.</param>
		private TypeDefinition CreateTypeFromElement(IMansionContext context, string name, XmlNamespaceManager nsmgr, XmlNode element)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="nsmgr">The <see cref="XmlNamespaceManager"/>.</param>
		/// <param name="descriptorNode">The <see cref="XmlNode"/> from which to load the descriptor.</param>
		/// <param name="definition">The <see cref="TypeDefinition"/>.</param>
		/// <returns>Returns the loaded <see cref="IDescriptor"/>.</returns>
		private static IDescriptor LoadDescriptor(IMansionContext context, XmlNamespaceManager nsmgr, XmlNode descriptorNode, TypeDefinition definition)
		{
			// construct the descriptor from XML
			var descriptor = XmlDescriptorFactory<TypeDescriptor>.Create(context, descriptorNode, init => init.TypeDefinition = definition);

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
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ICachingService cachingService;
		private readonly XmlReaderSettings readerSettings = new XmlReaderSettings
		                                                    {
		                                                    	ValidationType = ValidationType.Schema
		                                                    };
		private int initializedState;
		private ITypeDefinition rootType;
		private IDictionary<string, ITypeDefinition> types;
		#endregion
	}
}