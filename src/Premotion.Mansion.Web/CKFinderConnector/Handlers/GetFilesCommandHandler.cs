using System;
using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Handles the CKFinder GetFiles command.
	/// </summary>
	public class GetFilesCommandHandler : CurrentFolderCommandHandlerBase
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
			// create the files element
			var filesElement = connectorNode.Append(new XElement("Files"));

			// loop over all the folders beneath the current folder
			foreach (var child in AssetService.RetrieveEntries(context, CurrentAssetFolder))
			{
				// determine the size
				var fileSize = Math.Round(child.Size/1024f);
				if (fileSize < 1 && child.Size != 0)
					fileSize = 1;

				var fileElement = filesElement.Append(new XElement("File"));
				fileElement.SetAttributeValue("name", child.Name);
				fileElement.SetAttributeValue("date", child.ModificationDate.ToString("yyyyMMddHHmm"));
				fileElement.SetAttributeValue("size", fileSize);
			}
		}
		#endregion
	}
}