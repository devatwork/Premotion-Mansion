using System.Collections.Generic;

namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents the resource service for application resources.
	/// </summary>
	public interface IApplicationResourceService : IResourceService
	{
		#region Resource Methods
		/// <summary>
		/// Gets the resource from it's path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns the resource.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		IResource GetSingle(IMansionContext context, IResourcePath path);
		/// <summary>
		/// Gets the resources from their path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <returns>Returns the resources.</returns>
		/// <exception cref="ResourceNotFoundException">Thrown when a resource can not be resolved from it's path.</exception>
		IEnumerable<IResource> Get(IMansionContext context, IResourcePath path);
		/// <summary>
		/// Tries to get resources based on the path. When not resource is found no exception is thrown.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resources.</param>
		/// <param name="resources">The resources found.</param>
		/// <returns>Returns true when a resource was found, otherwise false.</returns>
		bool TryGet(IMansionContext context, IResourcePath path, out IEnumerable<IResource> resources);
		#endregion
		#region Folder Methods
		/// <summary>
		/// Enumerates the folders of a particular path
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path which to enumerate.</param>
		/// <returns>Returns the names of the folders.</returns>
		IEnumerable<string> EnumeratorFolders(IMansionContext context, string path);
		#endregion
	}
}