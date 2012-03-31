namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents the resource service for content resources.
	/// </summary>
	public interface IContentResourceService : IResourceService
	{
		#region Resource Methods
		/// <summary>
		/// Opens the resource using the specified path. This will create the resource if it does not already exist.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/> identifying the resource.</param>
		/// <returns>Returns the <see cref="IResource"/>.</returns>
		IResource GetResource(IMansionContext context, IResourcePath path);
		#endregion
	}
}