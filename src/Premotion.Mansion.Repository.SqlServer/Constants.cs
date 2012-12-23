namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Represents constants.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Defines the key in which the SQL-server connnection string is stored in the application settings.
		/// </summary>
		public const string ConnectionStringApplicationSettingKey = @"SQLSERVER_CONNECTION_STRING";
		/// <summary>
		/// The namespace in which the SQL Server tags live.
		/// </summary>
		public const string TagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/sqlserverrepository.tags.xsd";
		/// <summary>
		/// The namespace in which the SQL Server descriptors live.
		/// </summary>
		public const string DescriptorNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/sqlserverrepository.descriptors.xsd";
	}
}