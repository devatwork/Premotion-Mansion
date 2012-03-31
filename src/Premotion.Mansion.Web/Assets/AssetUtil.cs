using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Contains asset utility methods.
	/// </summary>
	public static class AssetUtil
	{
		/// <summary>
		/// Builds the asset path of the specified <paramref name="folder"/>.
		/// </summary>
		/// <param name="folder">The <see cref="AssetFolder"/> for which to create the path.</param>
		/// <returns>Returns the path.</returns>
		public static string GetPath(AssetFolder folder)
		{
			// validate arguments
			if (folder == null)
				throw new ArgumentNullException("folder");

			// build the url
			return GetPath(folder.AssetType.Node, folder.Node);
		}
		/// <summary>
		/// Builds the asset path of the specified <paramref name="folderNode"/>.
		/// </summary>
		/// <param name="assetTypeNode">The asset type node.</param>
		/// <param name="folderNode">The asset folder node.</param>
		/// <returns>Returns the path.</returns>
		public static string GetPath(Node assetTypeNode, Node folderNode)
		{
			// validate arguments
			if (assetTypeNode == null)
				throw new ArgumentNullException("assetTypeNode");
			if (folderNode == null)
				throw new ArgumentNullException("folderNode");

			// build the url
			return string.Join("/", new[] {"Assets"}.Concat(folderNode.Pointer.Path.Skip(assetTypeNode.Pointer.Depth - 1)));
		}
		/// <summary>
		/// Builds the folder url of the specified <paramref name="folderNode"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="assetTypeNode">The asset type node.</param>
		/// <param name="folderNode">The asset folder node.</param>
		/// <returns>Returns the <see cref="Uri"/>.</returns>
		public static Uri GetUrl(IMansionContext context, Node assetTypeNode, Node folderNode)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (assetTypeNode == null)
				throw new ArgumentNullException("assetTypeNode");
			if (folderNode == null)
				throw new ArgumentNullException("folderNode");

			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// get the path
			var folderPath = GetPath(assetTypeNode, folderNode);

			// create the relative path
			var prefixedRelativePath = HttpUtilities.CombineIntoRelativeUrl(webContext.HttpContext.Request.ApplicationPath, Constants.StaticContentPrefix, folderPath);

			// create the uri
			return new Uri(webContext.ApplicationBaseUri, prefixedRelativePath);
		}
	}
}