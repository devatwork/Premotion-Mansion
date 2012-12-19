using System;
using System.Configuration;
using RestSharp;

namespace Premotion.Mansion.Repository.ElasticSearch.Connection
{
	/// <summary>
	/// Implements the ElasticSearch connection manager.
	/// </summary>
	public class ConnectionManager
	{
		#region Constructors
		/// <summary>
		/// Constructs an ElasticSearch connection manager.
		/// </summary>
		public ConnectionManager()
		{
			// get the connection string
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			if (string.IsNullOrEmpty(connectionString))
				throw new InvalidOperationException("No valid value found in application settings for key " + Constants.ConnectionStringApplicationSettingKey);

			// create the client
			client = new RestClient(connectionString);
		}
		#endregion
		#region Private Fields
		private readonly RestClient client;
		#endregion
	}
}