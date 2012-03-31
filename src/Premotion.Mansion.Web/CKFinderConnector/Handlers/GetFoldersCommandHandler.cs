using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Handles the CKFinder GetFolders command.
	/// </summary>
	public class GetFoldersCommandHandler : CurrentFolderCommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="IMansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <param name="currentFolderElement">The <see cref="XElement"/> of the current folder node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(IMansionWebContext context, XElement connectorNode, XElement currentFolderElement)
		{
			// create the folder element
			var foldersElement = connectorNode.Append(new XElement("Folders"));

			// loop over all the folders beneath the current folder
			foreach (var child in AssetService.RetrieveFolders(context, CurrentAssetFolder))
			{
				var folderElement = foldersElement.Append(new XElement("Folder"));
				folderElement.SetAttributeValue("name", child.Label);
				folderElement.SetAttributeValue("hasChildren", true);
				folderElement.SetAttributeValue("acl", GetACLMask(child));
			}
		}
		#endregion
	}
}