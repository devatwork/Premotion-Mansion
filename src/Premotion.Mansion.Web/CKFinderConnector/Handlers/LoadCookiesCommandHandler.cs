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
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(MansionWebContext context, XElement connectorNode)
		{
			// create the cookies element
			var cookiesElement = connectorNode.Append(new XElement("Cookies"));

			// loop over all the cookies
			var cookies = Request.Cookies;
			for (var i = 0; i < cookies.Count; i++)
			{
				var cookie = cookies[i];
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