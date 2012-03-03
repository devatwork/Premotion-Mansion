using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Handles the CKFinder CreateFolder command.
	/// </summary>
	public class CreateFolderCommandHandler : CurrentFolderCommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <param name="currentFolderElement">The <see cref="XElement"/> of the current folder node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(MansionWebContext context, XElement connectorNode, XElement currentFolderElement)
		{
			// get the new folder name
			var folderName = (Request.QueryString["newFolderName"] ?? string.Empty);

			// create the folder
			var createdFolder = AssetService.CreateFolder(context, CurrentAssetFolder, folderName);

			// append the new folder response
			connectorNode.Append(new XElement("NewFolder")).SetAttributeValue("name", createdFolder.Label);
		}
		#endregion
	}
}