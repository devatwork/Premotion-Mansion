using System;
using System.Configuration;

namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Provides elastic search configuration methods.
	/// </summary>
	public static class Configuration
	{
		#region Static Flags
		/// <summary>
		/// Gets a flag indicating whether ElasticSearch is enabled or not.
		/// </summary>
		public static bool IsEnabled
		{
			get { return IsEnabledLoader.Value && !string.IsNullOrEmpty(ConnectionString); }
		}
		/// <summary>
		/// Gets the ElasticSearch conneciton string.
		/// </summary>
		public static string ConnectionString
		{
			get { return ConnectionStringLoader.Value; }
		}
		#endregion
		#region Private Fields
		private static readonly Lazy<bool> IsEnabledLoader = new Lazy<bool>(() => {
			// get the settings
			var appSettings = ConfigurationManager.AppSettings;

			// read the key
			return (appSettings[Constants.EnabledSettingKey] ?? string.Empty).Equals("true", StringComparison.OrdinalIgnoreCase);
		});
		private static readonly Lazy<string> ConnectionStringLoader = new Lazy<string>(() => {
			// get the connection string
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			return string.IsNullOrEmpty(connectionString) ? null : connectionString;
		});
		#endregion
	}
}