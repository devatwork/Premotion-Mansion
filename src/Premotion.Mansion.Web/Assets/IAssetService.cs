using System.Collections.Generic;
using System.IO;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Represents the asset service.
	/// </summary>
	public interface IAssetService
	{
		#region Asset Type Methods
		/// <summary>
		/// Retrieves all the asset types known by this service.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns all the asset types.</returns>
		IEnumerable<AssetType> RetrieveAssetTypes(IMansionContext context);
		/// <summary>
		/// Parses the resource type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the resource type.</param>
		/// <returns>Returns the resource type.</returns>
		AssetType ParseResourceType(IMansionContext context, string typeName);
		#endregion
		#region Asset Folder Methods
		/// <summary>
		/// Retrieves all the <see cref="AssetFolder"/>s underneath the <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="folder">The <see cref="AssetFolder"/> from which to get the children.</param>
		/// <returns>Returns all the <see cref="AssetFolder"/>s.</returns>
		IEnumerable<AssetFolder> RetrieveFolders(IMansionContext context, AssetFolder folder);
		/// <summary>
		/// Parses the <paramref name="path"/> into a <see cref="AssetFolder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="assetType">The <see cref="AssetType"/>.</param>
		/// <param name="path">The path which to parse.</param>
		/// <returns>Returns the <see cref="AssetFolder"/>.</returns>
		AssetFolder ParseFolder(IMansionContext context, AssetType assetType, string path);
		/// <summary>
		/// Creates an <see cref="AssetFolder"/> with <paramref name="folderName"/> under <paramref name="parentFolder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parentFolder">The <see cref="AssetFolder"/>.</param>
		/// <param name="folderName">The name of the folder.</param>
		/// <returns>Returns the created <see cref="AssetFolder"/>.</returns>
		AssetFolder CreateFolder(IMansionContext context, AssetFolder parentFolder, string folderName);
		#endregion
		#region Asset Entry Methods
		/// <summary>
		/// Retrieves the <see cref="AssetEntry"/>s of the specified <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="folder">The <see cref="AssetFolder"/> from which to retrieve the <see cref="AssetEntry"/>s.</param>
		/// <returns>Returns all the <see cref="AssetEntry"/>s.</returns>
		IEnumerable<AssetEntry> RetrieveEntries(IMansionContext context, AssetFolder folder);
		/// <summary>
		/// Stores the resource in the specified <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.IMansionContext</param>
		/// <param name="folder">The <see cref="AssetFolder"/> in which to store the resource.</param>
		/// <param name="filename">The filename of the resource.</param>
		/// <param name="inputStream">The input <see cref="Stream"/> containg the content of the resource.</param>
		/// <returns>Returns the stored <see cref="AssetEntry"/>.</returns>
		AssetEntry StoreResource(IMansionContext context, AssetFolder folder, string filename, Stream inputStream);
		#endregion
	}
}