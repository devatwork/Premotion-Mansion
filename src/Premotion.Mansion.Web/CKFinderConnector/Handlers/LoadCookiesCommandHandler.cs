using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Load cookies command handler.
	/// </summary>
	public class LoadCookiesCommandHandler : XmlCommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="IMansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(IMansionWebContext context, XElement connectorNode)
		{
			// create the cookies element
			var cookiesElement = connectorNode.Append(new XElement("Cookies"));

			// loop over all the cookies
			var cookies = Request.Cookies;
			foreach (var cookie in cookies.Values)
			{
				// ignore the CKFinder cookies
				if (cookie.Name.StartsWith("CKFinder_"))
					continue;

				// add the cookie elemeen
				var cookieElement = cookiesElement.Append(new XElement("Cookie"));
				cookieElement.SetAttributeValue("name", cookie.Name);
				cookieElement.SetAttributeValue("value", cookie.Value);
			}
		}
		#endregion
	}
}