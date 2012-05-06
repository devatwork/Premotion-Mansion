using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Implements the <see cref="IReflectionService"/>.
	/// </summary>
	public class ReflectionService : IReflectionService
	{
		#region Implementation of IReflectionService
		/// <summary>
		/// Initializes the reflection service, which registers all the exported types with the given <paramref name="assemblies"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/>.</param>
		/// <param name="assemblies">The ordered set of <see cref="Assembly"/>s.</param>
		public void Initialize(IConfigurableNucleus nucleus, IEnumerable<Assembly> assemblies)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			if (assemblies == null)
				throw new ArgumentNullException("assemblies");

			// set values
			assemblies = assemblies.ToArray();
			Assemblies = new Stack<Assembly>(assemblies);
			AssembliesReversed = new Stack<Assembly>(assemblies);

			// scan all the assemblies and register the types in them
			Scan(nucleus, Assemblies);
		}
		/// <summary>
		/// Gets all the assemblies loaded by this application in a bottom-to-top order. The top assembly is the most specific assembly.
		/// </summary>
		public IEnumerable<Assembly> Assemblies { get; private set; }
		/// <summary>
		/// Gets all the assemblies loaded by this application in a top-to-bottom order. The top assembly is the most specific assembly.
		/// </summary>
		public IEnumerable<Assembly> AssembliesReversed { get; private set; }
		#endregion
		#region Scan Methods
		/// <summary>
		/// Scans the given <paramref name="assemblies"/> and register all the type marked with <see cref="ExportedAttribute"/> within the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the types.</param>
		/// <param name="assemblies">The <see cref="Assembly"/>s which to scan for types.</param>
		private static void Scan(IConfigurableNucleus nucleus, IEnumerable<Assembly> assemblies)
		{
			// validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			if (assemblies == null)
				throw new ArgumentNullException("assemblies");

			// select all the exported types
			var exportedTypes = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(candidate => !candidate.IsAbstract).SelectMany(type => type.GetCustomAttributes(typeof (ExportedAttribute), true).Select(attribute => new
			                                                                                                                                                                                                                       {
			                                                                                                                                                                                                                       	Type = type,
			                                                                                                                                                                                                                       	ExportedAttribute = (ExportedAttribute) attribute
			                                                                                                                                                                                                                       })).ToList();

			// select all the named types
			var namedTypes = exportedTypes.Where(candidate => candidate.ExportedAttribute is NamedAttribute).ToList();

			// remove all the named types from the exported types, group the remainder by their contract type and select only the first per contract type
			exportedTypes = exportedTypes.Except(namedTypes).ToList().GroupBy(candidate => candidate.Type.Name, (key, values) => values.First()).ToList();

			// deduplicate the named types by their name and select only the first one
			var deduplicatedNamedTypes = namedTypes.Select(type => new
			                                                       {
			                                                       	Name = StrongNameGenerator.Generate((NamedAttribute) type.ExportedAttribute),
			                                                       	type.Type,
			                                                       	type.ExportedAttribute
			                                                       }).GroupBy(candidate => candidate.Name, (key, values) => values.First(), StringComparer.OrdinalIgnoreCase).ToList();

			// find the methods method
			var createInstanceFactoryMethodInfo = typeof (Extensions).GetMethod("CreateInstanceFactory");
			if (createInstanceFactoryMethodInfo == null)
				throw new InvalidOperationException("Could not find CreateInstanceFactory method on Extensions");
			var registerWithKeyMethodInfo = typeof (IConfigurableNucleus).GetMethods().First(candidate => candidate.Name.Equals("Register") && candidate.GetParameters().Length == 2);
			if (registerWithKeyMethodInfo == null)
				throw new InvalidOperationException("Could not find Register with key method on IConfigurableNucleus");

			// register all the exported types
			foreach (var type in exportedTypes)
			{
				// create the factory
				var factoryInstance = createInstanceFactoryMethodInfo.MakeGenericMethod(type.ExportedAttribute.ContractType).Invoke(null, new object[] {type.Type});

				// register the component
				registerWithKeyMethodInfo.MakeGenericMethod(type.ExportedAttribute.ContractType).Invoke(nucleus, new[] {type.Type.FullName, factoryInstance});
			}

			// register all the named types
			foreach (var type in deduplicatedNamedTypes)
			{
				// create the factory
				var factoryInstance = createInstanceFactoryMethodInfo.MakeGenericMethod(type.ExportedAttribute.ContractType).Invoke(null, new object[] {type.Type});

				// register the component
				registerWithKeyMethodInfo.MakeGenericMethod(type.ExportedAttribute.ContractType).Invoke(nucleus, new[] {type.Name, factoryInstance});
			}
		}
		#endregion
	}
}