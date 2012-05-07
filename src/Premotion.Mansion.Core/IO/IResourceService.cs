namespace Premotion.Mansion.Core.IO
{
	/// <summary>
	/// Represents the base interface for resource services.
	/// </summary>
	public interface IResourceService
	{
		#region Resource Methods
		/// <summary>
		/// Checks whether a resource exists at the specified paths.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns true when a resource exists, otherwise false.</returns>
		bool Exists(IMansionContext context, IResourcePath path);
		#endregion
		#region Path Methods
		/// <summary>
		/// Parses the properties into a resource path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties which to parse.</param>
		/// <returns>Returns the parsed resource path.</returns>
		IResourcePath ParsePath(IMansionContext context, IPropertyBag properties);
		/// <summary>
		/// Gets the first and most important relative path of <paramref name="path"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns a string version of the most important relative path.</returns>
		string GetFirstRelativePath(IMansionContext context, IResourcePath path);
		#endregion
	}
}