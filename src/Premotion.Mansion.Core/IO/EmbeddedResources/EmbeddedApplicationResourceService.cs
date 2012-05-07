using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.IO.EmbeddedResources
{
	/// <summary>
	/// Implements the <see cref="IApplicationResourceService"/> for embedded resources.
	/// </summary>
	public class EmbeddedApplicationResourceService : IApplicationResourceService
	{
		#region Constructors
		/// <summary>
		/// Constructs the EmbeddedApplicationResourceService.
		/// </summary>
		/// <param name="resourceSubFolder">The subfolder in which the resources live.</param>
		/// <param name="pathInterpreters">The <see cref="IEnumerable{T}"/>s.</param>
		/// <param name="reflectionService">The <see cref="IReflectionService"/> holding the assemblies used to load the resources from.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="resourceSubFolder"/>, <paramref name="pathInterpreters"/> or <paramref name="reflectionService"/> is null.</exception>
		public EmbeddedApplicationResourceService(string resourceSubFolder, IEnumerable<ResourcePathInterpreter> pathInterpreters, IReflectionService reflectionService)
		{
			// validate arguments
			if (string.IsNullOrEmpty(resourceSubFolder))
				throw new ArgumentNullException("resourceSubFolder");
			if (pathInterpreters == null)
				throw new ArgumentNullException("pathInterpreters");
			if (reflectionService == null)
				throw new ArgumentNullException("reflectionService");

			// set values
			this.pathInterpreters = pathInterpreters.ToArray();

			// translate the resource sub folder
			this.resourceSubFolder = TranslatePath(resourceSubFolder);

			// first select all the resource names from all assemblies
			var allResourceNames = reflectionService.AssembliesReversed.SelectMany(assembly =>
			                                                                       {
			                                                                       	// get all the resources in this assembly
			                                                                       	var resourceNames = assembly.GetManifestResourceNames();

			                                                                       	// remove the assembly name from each resource.
			                                                                       	var assemblyNameLength = assembly.GetName().Name.Length + 1 + this.resourceSubFolder.Length + 1;
			                                                                       	var normalizedResourceNames = resourceNames.Select(resourceName => resourceName.Substring(assemblyNameLength));

			                                                                       	// return the assembly and the normalized resource name
			                                                                       	return normalizedResourceNames.Select(resourceName => new
			                                                                       	                                                      {
			                                                                       	                                                      	Assembly = assembly,
			                                                                       	                                                      	ResourceName = resourceName
			                                                                       	                                                      });
			                                                                       }
				);

			// group all the assemblies by the common resource names
			var groupedResourceNames = allResourceNames.GroupBy(resourceName => resourceName.ResourceName, StringComparer.OrdinalIgnoreCase);

			// turn the grouped resource name into a lookup table
			lookupTable = groupedResourceNames.ToDictionary(key => key.Key, group => group.Select(resource => resource.Assembly).ToArray(), StringComparer.OrdinalIgnoreCase);
		}
		#endregion
		#region Implementation of IResourceService
		/// <summary>
		/// Checks whether a resource exists at the specified paths.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns true when a resource exists, otherwise false.</returns>
		public bool Exists(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			// check if the lookup table contains any of the paths
			return path.Paths.Select(TranslatePath).Any(candidate => lookupTable.ContainsKey(candidate));
		}
		/// <summary>
		/// Parses the properties into a resource path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties which to parse.</param>
		/// <returns>Returns the parsed resource path.</returns>
		public IResourcePath ParsePath(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// parse the paths
			return Election<ResourcePathInterpreter, IPropertyBag>.Elect(context, pathInterpreters, properties).Interpret(context, properties);
		}
		/// <summary>
		/// Gets the first and most important relative path of <paramref name="path"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns a string version of the most important relative path.</returns>
		public string GetFirstRelativePath(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			// return the first relative path
			return path.Paths.First();
		}
		#endregion
		#region Implementation of IApplicationResourceService
		/// <summary>
		/// Gets the resource from it's path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns the resource.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		public IResource GetSingle(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			return Get(context, path).Single();
		}
		/// <summary>
		/// Gets the resources from their path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <returns>Returns the resources.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		public IEnumerable<IResource> Get(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			// locate the resources by their path
			var resources = LocateResources(path).ToList();

			//if there are no resources for this path, throw an exception
			if (resources.Count == 0)
				throw new ResourceNotFoundException(new[] {path});

			// return the resouces
			return resources;
		}
		/// <summary>
		/// Tries to get resources based on the path. When not resource is found no exception is thrown.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <param name="resources">The resources found.</param>
		/// <returns>Returns true when a resource was found, otherwise false.</returns>
		public bool TryGet(IMansionContext context, IResourcePath path, out IEnumerable<IResource> resources)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			// locate the resources by their path
			var resourceList = LocateResources(path).ToList();
			resources = resourceList;

			// return the resouces
			return resourceList.Count > 0;
		}
		/// <summary>
		/// Enumerates the folders of a particular path
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path which to enumerate.</param>
		/// <returns>Returns the names of the folders.</returns>
		public IEnumerable<string> EnumeratorFolders(IMansionContext context, string path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");

			// translate the path
			var translatedPath = TranslatePath(path);

			// find all the resources starting with the given path
			var resourcesStartingWithPath = lookupTable.Keys.Where(candidate => candidate.StartsWith(translatedPath, StringComparison.OrdinalIgnoreCase));

			// filter out the only the folder in which the resource is located
			resourcesStartingWithPath = resourcesStartingWithPath.Select(candidate => candidate.Substring(translatedPath.Length + 1).Split(new[] {'.'}).First());

			// return the paths
			return resourcesStartingWithPath.Distinct(StringComparer.OrdinalIgnoreCase);
		}
		#endregion
		#region Private Methods
		/// <summary>
		/// Translates the incoming <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The path which to translate.</param>
		/// <returns>Returns the translated path.</returns>
		private static string TranslatePath(string path)
		{
			// validate arguments
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");

			return path.Replace('/', '.').Replace('\\', '.').TrimStart('.');
		}
		/// <summary>
		/// Locates all the <see cref="IResource"/>s for the given <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns the resource.</returns>
		private IEnumerable<IResource> LocateResources(IResourcePath path)
		{
			// loop over all the paths
			foreach (var translatedPath in path.Paths.Select(TranslatePath))
			{
				//  check if the resource exists
				Assembly[] assemblies;
				if (!lookupTable.TryGetValue(translatedPath, out assemblies))
					continue;

				// if the path is not overridable just return the last resource
				if (!path.Overridable)
				{
					yield return new EmbeddedResource(resourceSubFolder + "." + translatedPath, assemblies.Last(), path);
					yield break;
				}

				// loop over all the assemblies to open the resources
				foreach (var assembly in assemblies)
					yield return new EmbeddedResource(resourceSubFolder + "." + translatedPath, assembly, path);
			}
		}
		#endregion
		#region Private Fields
		private readonly IDictionary<string, Assembly[]> lookupTable;
		private readonly IEnumerable<ResourcePathInterpreter> pathInterpreters;
		private readonly string resourceSubFolder;
		#endregion
	}
}