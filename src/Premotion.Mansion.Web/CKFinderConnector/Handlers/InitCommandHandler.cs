using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Handles the CKFinder Init command.
	/// </summary>
	public class InitCommandHandler : XmlCommandHandlerBase
	{
		#region Constants
		private const string KeyChars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
		#endregion
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(MansionWebContext context, XElement connectorNode)
		{
			// add the connector info node
			var connectorInfoNode = connectorNode.Append(new XElement("ConnectorInfo"));
			connectorInfoNode.SetAttributeValue("enabled", true);
			connectorInfoNode.SetAttributeValue("thumbsEnabled", false);
			connectorInfoNode.SetAttributeValue("uploadCheckImages", true);
			connectorInfoNode.SetAttributeValue("thumbsUrl", "/userfiles/_thumbs/");
			connectorInfoNode.SetAttributeValue("thumbsDirectAccess", false);
			connectorInfoNode.SetAttributeValue("imgWidth", "1600");
			connectorInfoNode.SetAttributeValue("imgHeight", "1200");

			// add licence information
			var licenceName = string.Empty;
			var licenceKey = string.Empty.PadRight(32, ' ');
			var index = KeyChars.IndexOf(licenceKey[0])%5;
			if (1 == index || 4 == index)
				licenceName = string.Empty;
			connectorInfoNode.SetAttributeValue("s", licenceName);
			connectorInfoNode.SetAttributeValue("c", string.Concat(licenceKey[11], licenceKey[0], licenceKey[8], licenceKey[12], licenceKey[26], licenceKey[2], licenceKey[3], licenceKey[25], licenceKey[1]).Trim());

			// add the resource types node
			var resourceTypesNode = connectorNode.Append(new XElement("ResourceTypes"));

			// loop over all the asset types
			foreach (var assetType in AssetService.RetrieveAssetTypes(context))
			{
				var resourceTypeNode = resourceTypesNode.Append(new XElement("ResourceType"));
				resourceTypeNode.SetAttributeValue("name", assetType.Label);
				resourceTypeNode.SetAttributeValue("url", FormatUri(assetType.Url));
				resourceTypeNode.SetAttributeValue("maxSize", "0");
				resourceTypeNode.SetAttributeValue("allowedExtensions", string.Empty);
				resourceTypeNode.SetAttributeValue("deniedExtensions", string.Empty);
				resourceTypeNode.SetAttributeValue("hash", GetMACTripleDESHash(assetType.BasePath));
				resourceTypeNode.SetAttributeValue("hasChildren", true);
				resourceTypeNode.SetAttributeValue("acl", GetACLMask(assetType));
			}

			// add the plugin info
			connectorNode.Append(new XElement("PluginsInfo"));
		}
		#endregion
	}
}