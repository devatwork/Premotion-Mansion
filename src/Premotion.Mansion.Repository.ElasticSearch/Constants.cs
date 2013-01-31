namespace Premotion.Mansion.Repository.ElasticSearch
{
	/// <summary>
	/// Represents constants.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The namespace in which the ElasticSearch descriptors live.
		/// </summary>
		public const string DescriptorNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/elasticsearch.descriptors.xsd";
		/// <summary>
		/// The namespace in which the ElasticSearch tags live.
		/// </summary>
		public const string TagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/elasticsearch.tags.xsd";
		/// <summary>
		/// Defines the key in which the ElasticSearch connnection string is stored in the application settings.
		/// </summary>
		public const string ConnectionStringApplicationSettingKey = "SEARCHBOX_URL";
		/// <summary>
		/// Represents the setting key for enabling elastic search service.
		/// </summary>
		public const string EnabledSettingKey = "ELASTIC-SEARCH-ENABLED";
	}
}