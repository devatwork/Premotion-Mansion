namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Defines constants for the application settings.
	/// </summary>
	public static class ApplicationSettingsConstants
	{
		/// <summary>
		/// Defines the name of the dataspace in which the application settings are available.
		/// </summary>
		public const string DataspaceName = "Application";
		/// <summary>
		/// Application setting indicating whether the application is live (true) or in staging mode (false).
		/// </summary>
		public const string IsLiveApplicationSetting = "APPLICATION_IS_LIVE";
		/// <summary>
		/// Application setting indicating which repository to load.
		/// </summary>
		public const string RepositoryNamespace = "REPOSITORY_NAMESPACE";
	}
}