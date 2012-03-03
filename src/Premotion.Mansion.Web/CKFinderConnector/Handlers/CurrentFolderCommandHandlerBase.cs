using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Base class for folder command handlers.
	/// </summary>
	public abstract class CurrentFolderCommandHandlerBase : XmlCommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override sealed void DoHandle(MansionWebContext context, XElement connectorNode)
		{
			// set the resource type attribtute
			connectorNode.SetAttributeValue("resourceType", AssetType.Label);

			// add current folder
			var currentFolderElement = connectorNode.Append(new XElement("CurrentFolder"));
			currentFolderElement.SetAttributeValue("path", CurrentAssetFolder.Path);
			currentFolderElement.SetAttributeValue("url", FormatUri(CurrentAssetFolder.Url));
			currentFolderElement.SetAttributeValue("acl", GetACLMask(CurrentAssetFolder));

			// invoke the template method
			DoHandle(context, connectorNode, currentFolderElement);
		}
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <param name="currentFolderElement">The <see cref="XElement"/> of the current folder node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected abstract void DoHandle(MansionWebContext context, XElement connectorNode, XElement currentFolderElement);
		#endregion
	}
}