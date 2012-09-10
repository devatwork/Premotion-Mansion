using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Dynamo;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents the factory for application's <see cref="IMansionContext"/>.
	/// </summary>
	public static class MansionWebApplicationContextFactory
	{
		#region Nested type: ContextContainer
		/// <summary>
		/// Contains the contexts of the current application.
		/// </summary>
		private class ContextContainer
		{
			#region Constructors
			/// <summary>
			/// 
			/// </summary>
			/// <param name="applicationContext"></param>
			/// <param name="lastAssemblyWriteTime"></param>
			public ContextContainer(IMansionContext applicationContext, DateTime lastAssemblyWriteTime)
			{
				// validate arguments
				if (applicationContext == null)
					throw new ArgumentNullException("applicationContext");

				// set values
				this.applicationContext = applicationContext;
				this.lastAssemblyWriteTime = lastAssemblyWriteTime;
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets the <see cref="ApplicationContext"/>.
			/// </summary>
			public IMansionContext ApplicationContext
			{
				get { return applicationContext; }
			}
			/// <summary>
			/// Gets the write time of the newest assembly.
			/// </summary>
			public DateTime LastAssemblyWriteTime
			{
				get { return lastAssemblyWriteTime; }
			}
			#endregion
			#region Private Fields
			private readonly IMansionContext applicationContext;
			private readonly DateTime lastAssemblyWriteTime;
			#endregion
		}
		#endregion
		#region Singleton Implementation
		/// <summary>
		/// Creates a single instance of the <see cref="MansionWebApplicationContextFactory"/> class.
		/// </summary>
		private static readonly Lazy<ContextContainer> InstanceFactory = new Lazy<ContextContainer>(() =>
		                                                                                            {
		                                                                                            	// make sure there is an hosted environment
		                                                                                            	if (!HostingEnvironment.IsHosted)
		                                                                                            		throw new InvalidOperationException("Premotion Mansion Web framework can only run within a hosted environment");

		                                                                                            	// create a nucleus
		                                                                                            	var nucleus = new DynamoNucleusAdapter();
		                                                                                            	nucleus.Register<IReflectionService>(t => new ReflectionService());

		                                                                                            	// create the application context
		                                                                                            	var applicationContext = new MansionContext(nucleus);

		                                                                                            	// assemblies
		                                                                                            	var assemblies = LoadOrderedAssemblyList().ToList();

		                                                                                            	// register all the types within the assembly
		                                                                                            	nucleus.ResolveSingle<IReflectionService>().Initialize(nucleus, assemblies);

		                                                                                            	// calculate the last application modification time
		                                                                                            	var lastAssemblyWriteTime = assemblies.Max(assembly => new FileInfo(assembly.Location).LastWriteTime);

		                                                                                            	// get all the application bootstrappers from the nucleus and allow them to bootstrap the application
		                                                                                            	foreach (var bootstrapper in nucleus.Resolve<ApplicationBootstrapper>().OrderBy(bootstrapper => bootstrapper.Weight))
		                                                                                            		bootstrapper.Bootstrap(nucleus);

		                                                                                            	// compile the nucleus for ultra fast performance
		                                                                                            	nucleus.Optimize();

		                                                                                            	// get all the application initializers from the nucleus and allow them to initialize the application
		                                                                                            	foreach (var initializer in nucleus.Resolve<ApplicationInitializer>().OrderBy(initializer => initializer.Weight))
		                                                                                            		initializer.Initialize(applicationContext);

		                                                                                            	// return the container
		                                                                                            	return new ContextContainer(applicationContext, lastAssemblyWriteTime);
		                                                                                            });
		/// <summary>
		/// Gets the <see cref="IMansionContext"/>, which is the context of the entire application.
		/// </summary>
		public static IMansionContext Instance
		{
			get { return InstanceFactory.Value.ApplicationContext; }
		}
		#endregion
		#region Assembly Load Methods
		/// <summary>
		/// Loads an ordered list of assemblies.
		/// </summary>
		/// <returns>Returns the ordered list.</returns>
		private static IEnumerable<Assembly> LoadOrderedAssemblyList()
		{
			// find the directory containing the assemblies
			var binDirectory = HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.GetDirectoryName(typeof (MansionWebApplicationContextFactory).Assembly.Location);
			if (String.IsNullOrEmpty(binDirectory))
				throw new InvalidOperationException("Could not find bin directory containing the assemblies");

			// load the assemblies
			var assemblies = Directory.GetFiles(binDirectory, "*.dll").Select(Assembly.LoadFrom);

			//  filter the assembly list include only assemblies marked with the ScanAssemblyAttribute attribute
			assemblies = assemblies.Where(candidate => candidate.IsMansionAssembly());

			// create a list of assemblies with their assembly name and their dependencies
			var assembliesWithDependecies = assemblies.Select(assembly => new
			                                                              {
			                                                              	Assembly = assembly,
			                                                              	AssemblyName = assembly.GetName(),
			                                                              	Dependencies = assembly.GetReferencedAssemblies().Where(candidate => candidate.IsMansionAssembly()).ToArray()
			                                                              });

			// keep a list of all the resolved dependencies
			var resolved = new List<string>();

			// sort the assemblies topological
			var sorted = assembliesWithDependecies.TopologicalSort(candidate =>
			                                                       {
			                                                       	// if there number of unresolved dependencies is greater than zero it is not ready to be resolved
			                                                       	if (candidate.Dependencies.Any(dependency => !resolved.Contains(dependency.Name, StringComparer.OrdinalIgnoreCase)))
			                                                       		return false;

			                                                       	// mark this assembly as resolved
			                                                       	resolved.Add(candidate.AssemblyName.Name);
			                                                       	return true;
			                                                       });

			// select only the assemblies
			return sorted.Select(x => x.Assembly);
		}
		#endregion
		#region Launch Methods
		/// <summary>
		/// Gets the <see cref="DateTime"/> when the application was last modified.
		/// </summary>
		public static DateTime ApplicationModifiedDate
		{
			get { return InstanceFactory.Value.LastAssemblyWriteTime; }
		}
		#endregion
	}
}