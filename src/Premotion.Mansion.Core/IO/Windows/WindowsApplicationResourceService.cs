using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.IO.Windows
{
	/// <summary>
	/// Implements <see cref="IApplicationResourceService"/> for windows applications.
	/// </summary>
	public class WindowsApplicationResourceService : ManagedLifecycleService, IApplicationResourceService, IServiceWithDependencies
	{
		#region Constructors
		/// <summary>
		/// Constructs the window resource service.
		/// </summary>
		/// <param name="physicalBasePath">The physical base path of th application.</param>
		/// <param name="resourceSubFolder">The subfolder in which the application paths live.</param>
		public WindowsApplicationResourceService(string physicalBasePath, string resourceSubFolder)
		{
			// validate arguments
			if (string.IsNullOrEmpty(physicalBasePath))
				throw new ArgumentNullException("physicalBasePath");

			// get the directory info
			var physicalBaseDirectory = new DirectoryInfo(physicalBasePath);
			if (!physicalBaseDirectory.Exists)
				throw new InvalidOperationException(string.Format("Physical path '{0}' does not exist", physicalBasePath));

			// set the value
			this.physicalBasePath = physicalBaseDirectory.FullName;
			this.resourceSubFolder = resourceSubFolder;
		}
		#endregion
		#region Implementation of IApplicationResourceService
		/// <summary>
		/// Checks whether a resource exists at the specified paths.
		/// </summary>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns true when a resource exists, otherwise false.</returns>
		public bool Exists(IResourcePath path)
		{
			return TryLocate(path).Any();
		}
		/// <summary>
		/// Gets the resource from it's path.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns the resource.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		public IResource GetSingle(IContext context, IResourcePath path)
		{
			return Get(context, path).Single();
		}
		/// <summary>
		/// Gets the resources from their path.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <returns>Returns the resources.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		public IEnumerable<IResource> Get(IContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// loop through all the paths>
			var count = 0;

			// return the resolved resources
			foreach (var physicalPath in TryLocate(path))
			{
				count++;
				yield return new FileResource(physicalPath, path);
			}

			// make sure at least one resource was found
			if (count == 0)
				throw new ResourceNotFoundException(new[] {path});
		}
		/// <summary>
		/// Tries to get resources based on the path. When not resource is found no exception is thrown.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <param name="resources">The resources found.</param>
		/// <returns>Returns true when a resource was found, otherwise false.</returns>
		public bool TryGet(IContext context, IResourcePath path, out IEnumerable<IResource> resources)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// retrieve the resources
			resources = (from physicalPath in TryLocate(path)
			             select new FileResource(physicalPath, path));

			// check if any resource was found
			return resources.Any(x => true);
		}
		/// <summary>
		/// Enumerates the folders of a particular path
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="path">The path which to enumerate.</param>
		/// <returns>Returns the names of the folders.</returns>
		public IEnumerable<string> EnumeratorFolders(IContext context, string path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// loop through all the paths
			return from rootPath in rootPaths
			       select new DirectoryInfo(ResourceUtils.Combine(rootPath, path))
			       into directoryInfo
			       where directoryInfo.Exists
			       from directory in directoryInfo.EnumerateDirectories()
			       where (directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (directory.Attributes & FileAttributes.System) != FileAttributes.System
			       select directory.Name;
		}
		#endregion
		#region Implementation of IResourceService
		/// <summary>
		/// Parses the properties into a resource path.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="properties">The properties which to parse.</param>
		/// <returns>Returns the parsed resource path.</returns>
		public IResourcePath ParsePath(IContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");
			CheckDisposed();

			return Election<ResourcePathInterpreter, IPropertyBag>.Elect(context, pathInterpreters, properties).Interpret(context, properties);
		}
		/// <summary>
		/// Gets the first and most important relative path of <paramref name="resourcePath"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="resourcePath">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns a string version of the most important relative path.</returns>
		public string GetFirstRelativePath(IContext context, IResourcePath resourcePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resourcePath == null)
				throw new ArgumentNullException("resourcePath");
			CheckDisposed();

			return TryLocate(resourcePath).First().FullName.Substring(physicalBasePath.Length);
		}
		#endregion
		#region Implementation of IStartableService
		/// <summary>
		/// Starts this service.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/> in which this service is started.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// loop over all the registerd assemblies to see if they define a path
			var assemblyRegistrationService = context.Nucleus.Get<IAssemblyRegistrationService>(context);
			foreach (var rootPath in assemblyRegistrationService.RegisteredAssemblies.Select(assemblyModel => ResourceUtils.Combine(physicalBasePath, assemblyModel.Name, resourceSubFolder)).Where(Directory.Exists))
			{
				// make sure the path is only added once
				if (rootPaths.Contains(rootPath))
					throw new InvalidOperationException(string.Format("The root path '{0}' is already added", rootPath));

				// add the root path
				rootPaths.Add(rootPath);
				rootPathsReverse.Insert(0, rootPath);
			}

			// get all the path interpreters
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);
			pathInterpreters.AddRange(objectFactoryService.Create<ResourcePathInterpreter>(namingService.Lookup<ResourcePathInterpreter>()));

			// add file system watchers
			var cacheService = context.Nucleus.Get<ICachingService>(context);
			foreach (var watcher in rootPaths.Select(rootPath => new FileSystemWatcher(rootPath)
			                                                     {
			                                                     	EnableRaisingEvents = true,
			                                                     	IncludeSubdirectories = true
			                                                     }))
			{
				watcher.Changed += (sender, e) =>
				                   {
				                   	// clear the cache when this object is not yet disposed
				                   	if (IsNotDisposed)
				                   		cacheService.ClearAll();
				                   };
				watchers.Add(watcher);
			}
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			if (!disposeManagedResources)
				return;

			// loop through all the watchers
			foreach (var watcher in watchers)
				watcher.Dispose();
			watchers.Clear();
		}
		#endregion
		#region Locate Methods
		/// <summary>
		/// Tries to locate a path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private IEnumerable<FileInfo> TryLocate(IResourcePath path)
		{
			// validate arguments
			if (path == null)
				throw new ArgumentNullException("path");
			if (path.Paths == null)
				throw new InvalidOperationException("A path must return an enumerable of paths");
			CheckDisposed();

			// get the appropriate path order
			IEnumerable<string> applicationPaths = path.Overridable ? rootPaths : rootPathsReverse;

			// loop through all the paths
			foreach (var fileInfo in path.Paths.SelectMany(resourcePath => applicationPaths.Select(applicationPath => ResourceUtils.Combine(applicationPath, resourcePath)).Select(candicatePath => new FileInfo(candicatePath)).Where(fileInfo => fileInfo.Exists)))
			{
				// return the found file
				yield return fileInfo;

				// check if 
				if (!path.Overridable)
					yield break;
			}
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ITypeDirectoryService>().Add<IObjectFactoryService>().Add<ICachingService>();
		private readonly List<ResourcePathInterpreter> pathInterpreters = new List<ResourcePathInterpreter>();
		private readonly string physicalBasePath;
		private readonly string resourceSubFolder;
		private readonly List<string> rootPaths = new List<string>();
		private readonly List<string> rootPathsReverse = new List<string>();
		private readonly List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
		#endregion
	}
}