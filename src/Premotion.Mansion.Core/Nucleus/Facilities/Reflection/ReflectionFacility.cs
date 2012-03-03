using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Nucleus.Extension;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Reflection
{
	/// <summary>
	/// This facility orchestrates the <see cref="IAssemblyRegistrationService"/>, <see cref="ITypeDirectoryService"/> and <see cref="IObjectFactoryService"/> services.
	/// </summary>
	public class ReflectionFacility : FacilityBase, IAssemblyRegistrationService, ITypeDirectoryService, IObjectFactoryService
	{
		#region Nested type: NamedType
		/// <summary>
		/// Represents a named type.
		/// </summary>
		private class NamedType
		{
			#region Constructors
			/// <summary>
			/// Constructs a named type.
			/// </summary>
			/// <param name="namespaceUri">The namespace of the identifier.</param>
			/// <param name="name">The name of the identifier.</param>
			/// <param name="targetType">The target type of the named object.</param>
			public NamedType(string namespaceUri, string name, Type targetType)
			{
				// validate arguments
				if (string.IsNullOrEmpty(namespaceUri))
					throw new ArgumentNullException("namespaceUri");
				if (string.IsNullOrEmpty(name))
					throw new ArgumentNullException("name");
				if (targetType == null)
					throw new ArgumentNullException("targetType");

				// set values
				NamespaceUri = namespaceUri;
				Name = name;
				TargetType = targetType;
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the name.
			/// </summary>
			public string Name { get; private set; }
			/// <summary>
			/// Gets the namespace uri.
			/// </summary>
			public string NamespaceUri { get; private set; }
			/// <summary>
			/// Get the type of the object which is named.
			/// </summary>
			public Type TargetType { get; private set; }
			#endregion
		}
		#endregion
		#region Overrides of FacilityBase
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		protected override void DoActivate(IContext context, IExtendedNucleus nucleus)
		{
			// register the reflection services
			nucleus.Register<IAssemblyRegistrationService>(context, this);
			nucleus.Register<ITypeDirectoryService>(context, this);
			nucleus.Register<IObjectFactoryService>(context, this);
		}
		#endregion
		#region Implementation of IAssemblyRegistrationService
		/// <summary>
		/// Registers a <paramref name="model"/> of an <see cref="Assembly"/> in this service.
		/// </summary>
		/// <param name="model">The <see cref="AssemblyModel"/> which to register.</param>
		public void RegisterAssembly(AssemblyModel model)
		{
			// validate arguments
			if (model == null)
				throw new ArgumentNullException("model");
			CheckDisposed();

			// add the assembly model
			assemblyModels.Add(model);

			// get the exported public types from the assembly
			var assemblyExportedTypes = model.Assembly.GetTypes().Where(candidate => !candidate.IsAbstract && !candidate.IsGenericParameter && candidate.GetCustomAttributes(typeof (ExportedAttribute), true).Length > 0);

			// add all the types
			registeredTypes.AddRange(assemblyExportedTypes);

			// add all the named types
			namedTypes.AddRange(from exportedType in assemblyExportedTypes
			                    from namedAttribute in exportedType.GetCustomAttributes(typeof (NamedAttribute), true).Cast<NamedAttribute>()
			                    select new NamedType(namedAttribute.NamespaceUri, namedAttribute.Name, exportedType));
		}
		/// <summary>
		/// Gets the <see cref="AssemblyModel"/>s registered by this service.
		/// </summary>
		public IEnumerable<AssemblyModel> RegisteredAssemblies
		{
			get { return assemblyModels; }
		}
		#endregion
		#region Implementation of ITypeDirectoryService
		///<summary>
		/// Looks up all the types matching the types.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		///<returns>Returns all the matching types.</returns>
		public IEnumerable<Type> Lookup<TType>()
		{
			CheckDisposed();

			// look up the types
			return registeredTypes.Where(candidate => typeof (TType).IsAssignableFrom(candidate));
		}
		///<summary>
		/// Looks up all the types matching the types.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		/// <param name="namespaceUri">The namespace in which the types live.</param>
		///<returns>Returns all the matching types.</returns>
		public IEnumerable<Type> Lookup<TType>(string namespaceUri)
		{
			// validate arguments
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			CheckDisposed();

			// look up the types
			return namedTypes.Where(candidate => namespaceUri.Equals(candidate.NamespaceUri, StringComparison.OrdinalIgnoreCase) && typeof (TType).IsAssignableFrom(candidate.TargetType)).Select(x => x.TargetType);
		}
		///<summary>
		/// Tries to look up a type by it's <paramref name="namespaceUri"/> and <paramref name="name"/>.
		///</summary>
		///<typeparam name="TType">The <see cref="Type"/> which to get.</typeparam>
		/// <param name="namespaceUri">The namespace in which the types live.</param>
		/// <param name="name">The name of the type.</param>
		/// <param name="type">The type.</param>
		///<returns>Returns true when a matching type was found otherwise false.</returns>
		public bool TryLookupSingle<TType>(string namespaceUri, string name, out Type type)
		{
			// validate arguments
			if (string.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			CheckDisposed();

			// look up the type
			type = namedTypes.Where(candidate => namespaceUri.Equals(candidate.NamespaceUri, StringComparison.OrdinalIgnoreCase) && name.Equals(candidate.Name, StringComparison.OrdinalIgnoreCase) && typeof (TType).IsAssignableFrom(candidate.TargetType)).Select(x => x.TargetType).SingleOrDefault();
			return type != null;
		}
		#endregion
		#region Implementation of IObjectFactoryService
		/// <summary>
		/// Creates instances of the specified <paramref name="types"/>.
		/// </summary>
		/// <typeparam name="TType">The type of object.</typeparam>
		/// <param name="types">The <see cref="Type"/>s which to create instances for.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns>Returns the created instance.</returns>
		public IEnumerable<TType> Create<TType>(IEnumerable<Type> types, params object[] constructorParameters)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");
			if (constructorParameters == null)
				throw new ArgumentNullException("constructorParameters");
			CheckDisposed();

			// instantiate the types
			return types.Select(type => Create<TType>(type, constructorParameters));
		}
		/// <summary>
		/// Creates an instance of the specified <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TType">The type of object.</typeparam>
		/// <param name="type">The <see cref="Type"/> for which to create an instance.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns>Returns the created instance.</returns>
		public TType Create<TType>(Type type, params object[] constructorParameters)
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (constructorParameters == null)
				throw new ArgumentNullException("constructorParameters");
			CheckDisposed();

			// instantiate the type
			return (TType) Activator.CreateInstance(type, constructorParameters);
		}
		#endregion
		#region Private Fields
		private readonly List<AssemblyModel> assemblyModels = new List<AssemblyModel>();
		private readonly List<NamedType> namedTypes = new List<NamedType>();
		private readonly List<Type> registeredTypes = new List<Type>();
		#endregion
	}
}