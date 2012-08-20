using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Assets
{
	/// <summary>
	/// Provides a default implementation of <see cref="IAssetService"/>.
	/// </summary>
	public class AssetService : IAssetService
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public AssetService(IContentResourceService contentResourceService)
		{
			// validate arguments
			if (contentResourceService == null)
				throw new ArgumentNullException("contentResourceService");

			// set values
			this.contentResourceService = contentResourceService;
		}
		#endregion
		#region Implementation of IAssetService
		/// <summary>
		/// Retrieves all the asset types known by this service.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns all the asset types.</returns>
		public IEnumerable<AssetType> RetrieveAssetTypes(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the index node
			var indexNode = RetrieveAssetIndexNode(context);

			// retrieve all the asset type nodes
			var assetTypeNodeset = RetrieveAssetTypeNodeset(context, indexNode);

			// loop over all the nodes and turn them into asset type
			return assetTypeNodeset.Nodes.Select(node => AssetType.Create(context, node));
		}
		/// <summary>
		/// Parses the resource type.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="typeName">The name of the resource type.</param>
		/// <returns>Returns the resource type.</returns>
		public AssetType ParseResourceType(IMansionContext context, string typeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(typeName))
				return AssetType.Unknown;

			// get the index node
			var indexNode = RetrieveAssetIndexNode(context);

			// retrieve all the asset type nodes
			var nodeset = RetrieveAssetTypeNodeset(context, indexNode);

			// loop over all the nodes and turn them into asset type
			var assetType = nodeset.Nodes.Where(x => x.Pointer.Name.Equals(typeName)).Select(x => AssetType.Create(context, x)).FirstOrDefault();
			if (assetType == null)
				throw new InvalidOperationException(string.Format("Could not find asset type '{0}'", typeName));
			return assetType;
		}
		/// <summary>
		/// Retrieves all the <see cref="AssetFolder"/>s underneath the <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="folder">The <see cref="AssetFolder"/> from which to get the children.</param>
		/// <returns>Returns all the <see cref="AssetFolder"/>s.</returns>
		public IEnumerable<AssetFolder> RetrieveFolders(IMansionContext context, AssetFolder folder)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (folder == null)
				throw new ArgumentNullException("folder");

			// retrieve all the asset type nodes
			var nodeset = RetrieveFolderNodeset(context, folder.Node);

			// loop over all the nodes and turn them into asset type
			return nodeset.Nodes.Select(node => AssetFolder.Create(context, folder.AssetType, node));
		}
		/// <summary>
		/// Parses the <paramref name="path"/> into a <see cref="AssetFolder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="assetType">The <see cref="AssetType"/>.</param>
		/// <param name="path">The path which to parse.</param>
		/// <returns>Returns the <see cref="AssetFolder"/>.</returns>
		public AssetFolder ParseFolder(IMansionContext context, AssetType assetType, string path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (assetType == null)
				throw new ArgumentNullException("assetType");
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException("path");

			// if the asset type is unknown, return unknown folder
			if (assetType.Equals(AssetType.Unknown))
				return AssetFolder.Unknown;

			// get the path parts
			var parts = path.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return AssetFolder.Create(context, assetType, assetType.Node);

			// retrieve the folder node
			var folderNode = RetrieveFolderByNameNode(context, assetType.Node, parts);

			// create the folder
			return AssetFolder.Create(context, assetType, folderNode);
		}
		/// <summary>
		/// Creates an <see cref="AssetFolder"/> with <paramref name="folderName"/> under <paramref name="parentFolder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="parentFolder">The <see cref="AssetFolder"/>.</param>
		/// <param name="folderName">The name of the folder.</param>
		/// <returns>Returns the created <see cref="AssetFolder"/>.</returns>
		public AssetFolder CreateFolder(IMansionContext context, AssetFolder parentFolder, string folderName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (parentFolder == null)
				throw new ArgumentNullException("parentFolder");
			if (string.IsNullOrEmpty(folderName))
				throw new ArgumentNullException("folderName");

			// sanitise the folder name
			folderName = SanitiseFolderName(folderName);
			if (string.IsNullOrEmpty(folderName))
				throw new ArgumentNullException("folderName");

			// create the folder node
			var folderNode = CreateFolderNode(context, parentFolder.Node, folderName);

			// create the folder
			return AssetFolder.Create(context, parentFolder.AssetType, folderNode);
		}
		/// <summary>
		/// Retrieves the <see cref="AssetEntry"/>s of the specified <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="folder">The <see cref="AssetFolder"/> from which to retrieve the <see cref="AssetEntry"/>s.</param>
		/// <returns>Returns all the <see cref="AssetEntry"/>s.</returns>
		public IEnumerable<AssetEntry> RetrieveEntries(IMansionContext context, AssetFolder folder)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (folder == null)
				throw new ArgumentNullException("folder");

			// retrieve all the asset type nodes
			var nodeset = RetrieveEntryNodeset(context, folder.Node);

			// loop over all the nodes and turn them into asset type
			return nodeset.Nodes.Select(node => AssetEntry.Create(context, folder, node));
		}
		/// <summary>
		/// Stores the resource in the specified <paramref name="folder"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.IMansionContext</param>
		/// <param name="folder">The <see cref="AssetFolder"/> in which to store the resource.</param>
		/// <param name="filename">The filename of the resource.</param>
		/// <param name="inputStream">The input <see cref="Stream"/> containg the content of the resource.</param>
		/// <returns>Returns the stored <see cref="AssetEntry"/>.</returns>
		public AssetEntry StoreResource(IMansionContext context, AssetFolder folder, string filename, Stream inputStream)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (folder == null)
				throw new ArgumentNullException("folder");
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("context");
			if (inputStream == null)
				throw new ArgumentNullException("inputStream");

			// sanitise the file name
			filename = SanitiseFileName(filename);

			// store the resource
			var resourcePath = contentResourceService.ParsePath(context, new PropertyBag
			                                                             {
			                                                             	{"category", AssetUtil.GetPath(folder)},
			                                                             	{"relativePath", filename}
			                                                             });
			var resource = contentResourceService.GetResource(context, resourcePath);
			using (var pipe = resource.OpenForWriting())
				inputStream.CopyTo(pipe.RawStream);

			// create the asset entry
			var entryNode = CreateEntryNode(context, folder.Node, filename, contentResourceService.GetFirstRelativePath(context, resourcePath), inputStream.Length);

			// create the entry
			return AssetEntry.Create(context, folder, entryNode);
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Makes the folder name safe.
		/// </summary>
		/// <param name="input">The folder name which to sanitise.</param>
		/// <returns>Returns the sanitised folder name.</returns>
		private static string SanitiseFolderName(string input)
		{
			return nameSanitiserRegex.Replace(input, string.Empty);
		}
		/// <summary>
		/// Makes the file name safe.
		/// </summary>
		/// <param name="input">The file name which to sanitise.</param>
		/// <returns>Returns the sanitised file name.</returns>
		private static string SanitiseFileName(string input)
		{
			// get the file base name
			var baseName = (Path.GetFileNameWithoutExtension(input) ?? string.Empty);
			var extension = (Path.GetExtension(input) ?? string.Empty);

			// sanitise both
			baseName = nameSanitiserRegex.Replace(baseName, string.Empty);
			extension = nameSanitiserRegex.Replace(extension, string.Empty);

			// validate
			if (string.IsNullOrEmpty(baseName) || string.IsNullOrEmpty(extension))
				throw new InvalidOperationException(string.Format("'{0}.{1}' is not a valid file name", baseName, extension));

			// reassemble the file name.
			return baseName + "." + extension;
		}
		#endregion
		#region Repository Methods
		/// <summary>
		/// Retrieves the root node.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static Node RetrieveRootNode(IMansionContext context)
		{
			return context.Repository.RetrieveSingleNode(context, new PropertyBag {{"id", 1}});
		}
		/// <summary>
		/// Retrieves the asset index node.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static Node RetrieveAssetIndexNode(IMansionContext context)
		{
			var rootNode = RetrieveRootNode(context);

			// check if the index node exists
			var repository = context.Repository;
			var indexNode = repository.RetrieveSingleNode(context, new PropertyBag
			                                                   {
			                                                   	{"parentSource", rootNode},
			                                                   	{"depth", "any"},
			                                                   	{"baseType", "AssetTypeIndex"},
			                                                   	{"status", "any"},
			                                                   	{"bypassAuthorization", true}
			                                                   });
			if (indexNode != null)
				return indexNode;

			// create the node
			indexNode = repository.CreateNode(context, rootNode, new PropertyBag
			                                            {
			                                            	{"name", "Assets"},
			                                            	{"type", "AssetTypeIndex"},
			                                            	{"approved", true},
			                                            });
			repository.CreateNode(context, indexNode, new PropertyBag
			                                            {
			                                            	{"name", "Images"},
			                                            	{"type", "AssetType"},
			                                            	{"approved", true},
			                                            });
			repository.CreateNode(context, indexNode, new PropertyBag
			                                            {
			                                            	{"name", "Files"},
			                                            	{"type", "AssetType"},
			                                            	{"approved", true},
			                                            });
			return indexNode;
		}
		/// <summary>
		/// Retrieves the assset type nodes.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="indexNode"></param>
		/// <returns></returns>
		private static Nodeset RetrieveAssetTypeNodeset(IMansionContext context, Node indexNode)
		{
			return context.Repository.RetrieveNodeset(context, new PropertyBag
			                                            {
			                                            	{"parentSource", indexNode},
			                                            	{"baseType", "AssetType"},
			                                            	{"status", "published"},
			                                            });
		}
		/// <summary>
		/// Retrieves the child folder nodes of the <paramref name="indexNode"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="indexNode"></param>
		/// <returns></returns>
		private Nodeset RetrieveFolderNodeset(IMansionContext context, Node indexNode)
		{
			return context.Repository.RetrieveNodeset(context, new PropertyBag
			                                            {
			                                            	{"parentSource", indexNode},
			                                            	{"baseType", "AssetFolder"},
			                                            	{"status", "published"},
			                                            });
		}
		/// <summary>
		/// Retrieves the child entries nodes of the <paramref name="indexNode"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="indexNode"></param>
		/// <returns></returns>
		private Nodeset RetrieveEntryNodeset(IMansionContext context, Node indexNode)
		{
			return context.Repository.RetrieveNodeset(context, new PropertyBag
			                                            {
			                                            	{"parentSource", indexNode},
			                                            	{"baseType", "AssetEntry"},
			                                            	{"status", "published"},
			                                            });
		}
		/// <summary>
		/// Retrieves the folder <see cref="Node"/> by it's <paramref name="path"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="indexNode"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		private Node RetrieveFolderByNameNode(IMansionContext context, Node indexNode, IEnumerable<string> path)
		{
			var folderIndexNode = indexNode;
			var repository = context.Repository;
			foreach (var folderName in path)
			{
				var folderNode = repository.RetrieveSingleNode(context, new PropertyBag
				                                                    {
				                                                    	{"parentSource", folderIndexNode},
				                                                    	{"baseType", "AssetFolder"},
				                                                    	{"status", "published"},
				                                                    	{"name", folderName},
				                                                    });
				if (folderNode == null)
					throw new InvalidOperationException(string.Format("Could not find folder '{0}' under '{1}'", folderName, folderIndexNode.Pointer.Path));

				//set the new index folder
				folderIndexNode = folderNode;
			}

			return folderIndexNode;
		}
		/// <summary>
		/// Creates the folder.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parentNode"></param>
		/// <param name="folderName"></param>
		/// <returns></returns>
		private Node CreateFolderNode(IMansionContext context, Node parentNode, string folderName)
		{
			return context.Repository.CreateNode(context, parentNode, new PropertyBag
			                                                      {
			                                                      	{"name", folderName},
			                                                      	{"type", "AssetFolder"},
			                                                      	{"approved", "1"},
			                                                      });
		}
		/// <summary>
		/// Creates the entry.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parentNode"></param>
		/// <param name="filename"></param>
		/// <param name="path"></param>
		/// <param name="size"> </param>
		/// <returns></returns>
		private Node CreateEntryNode(IMansionContext context, Node parentNode, string filename, string path, long size)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			return context.Repository.CreateNode(context, parentNode, new PropertyBag
			                                                      {
			                                                      	{"name", filename},
			                                                      	{"path", path},
			                                                      	{"size", size},
			                                                      	{"type", "AssetEntry"},
			                                                      	{"approved", "1"},
			                                                      });
		}
		#endregion
		#region Private Fields
		private static readonly Regex nameSanitiserRegex = new Regex(@"\W", RegexOptions.Compiled);
		private readonly IContentResourceService contentResourceService;
		#endregion
	}
}