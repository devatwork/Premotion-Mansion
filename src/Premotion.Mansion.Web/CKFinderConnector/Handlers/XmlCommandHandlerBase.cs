using System;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Base class for all XML CKFinder command handlers.
	/// </summary>
	public abstract class XmlCommandHandlerBase : CommandHandlerBase
	{
		#region Overrides of CommandHandlerBase
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected override void DoHandle(MansionWebContext context)
		{
			// always send xml encoded in UTF-8
			Response.ContentEncoding = Encoding.UTF8;
			Response.ContentType = "text/xml";

			// never cache the response
			Response.Cache.SetCacheability(HttpCacheability.NoCache);

			// create the XML document of the response
			var document = new XDocument(new XDeclaration("1.0", "utf-8", null));

			// create the connector node
			var connectorNode = document.Append(new XElement("Connector"));

			// create the error node
			var errorNode = connectorNode.Append(new XElement("Error"));

			// try to process the request
			try
			{
				// invoke the concrete class
				DoHandle(context, connectorNode);

				// set the success error code
				errorNode.SetAttributeValue("number", (int) ErrorCodes.None);
			}
			catch (ThreadAbortException)
			{
				// thread is aborted, so don't throw any new exceptions
			}
			catch (ConnectorException ex)
			{
				errorNode.SetAttributeValue("number", (int) ex.Number);
				if (ex.HasConnectorMessage)
					errorNode.SetAttributeValue("text", ex.ConnectorMessage);
			}
			catch (Exception)
			{
				errorNode.SetAttributeValue("number", (int) ErrorCodes.Unknown);
			}

			// write the document to the output
			document.Save(Response.Output, SaveOptions.DisableFormatting);
		}
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="MansionWebContext"/>.</param>
		/// <param name="connectorNode">The <see cref="XElement"/> of the connector node.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected abstract void DoHandle(MansionWebContext context, XElement connectorNode);
		#endregion
	}
}