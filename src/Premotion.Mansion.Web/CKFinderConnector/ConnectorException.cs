using System;

namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// Represents an CKFinder connector exception.
	/// </summary>
	public class ConnectorException : ApplicationException
	{
		#region Constructors
		/// <summary>
		/// Constructs a 
		/// </summary>
		/// <param name="number">The error number.</param>
		/// <param name="connectorMessage">The custom connector error message.</param>
		public ConnectorException(ErrorCodes number, string connectorMessage = null)
		{
			Number = number;
			ConnectorMessage = connectorMessage;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the error number
		/// </summary>
		public ErrorCodes Number { get; private set; }
		/// <summary>
		/// Gets a flag indicating wether this exception has a <see cref="ConnectorMessage"/>.
		/// </summary>
		public bool HasConnectorMessage
		{
			get { return string.IsNullOrEmpty(ConnectorMessage); }
		}
		/// <summary>
		/// Gets the connector message.
		/// </summary>
		public string ConnectorMessage { get; private set; }
		#endregion
	}
}