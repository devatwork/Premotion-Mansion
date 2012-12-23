using System;
using System.Configuration;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Bootstraps SQL-server enabled applications.
	/// </summary>
	public class SqlServerApplicationBootstrapper : ApplicationBootstrapper
	{
		#region Constructors
		/// <summary>
		/// </summary>
		public SqlServerApplicationBootstrapper() : base(50)
		{
		}
		#endregion
		#region Overrides of ApplicationBootstrapper
		/// <summary>
		/// Registers all the services used by the application in the <paramref name="nucleus"/>.
		/// </summary>
		/// <param name="nucleus">The <see cref="IConfigurableNucleus"/> in which to register the services used by the application.</param>
		protected override void DoBootstrap(IConfigurableNucleus nucleus)
		{
			// if elastic search is not configured, do not register its services
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			if (string.IsNullOrEmpty(connectionString))
				return;

			// register the SQL-server services.
			nucleus.Register(resolver => Create());
			nucleus.Register<BaseStorageEngine>(resolver => new SqlServerStorageEngine(resolver.ResolveSingle<SqlServerRepository>()));
			nucleus.Register<BaseQueryEngine>("sqlserver:queryengine", resolver => new SqlServerQueryEngine(resolver.ResolveSingle<SqlServerRepository>()));
		}
		#endregion
		#region Create Methods
		/// <summary>
		/// Creates an instance of <see cref="SqlServerRepository"/>.
		/// </summary>
		/// <returns>Returns the <see cref="SqlServerRepository"/>.</returns>
		private static SqlServerRepository Create()
		{
			// check for connection string
			var appSettings = ConfigurationManager.AppSettings;
			var connectionString = appSettings[Constants.ConnectionStringApplicationSettingKey];
			if (string.IsNullOrEmpty(connectionString))
				throw new InvalidOperationException("Could not open connection to SQL server repository without a connection string. Make sure the setting SQLSERVER_CONNECTION_STRING is specified.");

			// create the repository
			return new SqlServerRepository(connectionString);
		}
		#endregion
	}
}