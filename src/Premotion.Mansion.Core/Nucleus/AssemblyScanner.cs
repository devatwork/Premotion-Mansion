using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Premotion.Mansion.Core.Nucleus
{
	/// <summary>
	/// Scans assemblies and registers their type.
	/// </summary>
	public static class AssemblyScanner
	{
		#region Scan Methods
		/// <summary>
		/// Scans the given <paramref name="assemblies"/> and register all the type marked with <see cref="ExportedAttribute"/> within the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the types.</param>
		/// <param name="assemblies">The <see cref="Assembly"/>s which to scan for types.</param>
		public static void Scan(IConfigurableNucleus nucleus, IEnumerable<Assembly> assemblies)
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