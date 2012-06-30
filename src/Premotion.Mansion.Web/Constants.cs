namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Defines constants for the web application library.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Gets the name of the backoffice user revival data cookie.
		/// </summary>
		public const string BackofficeUserRevivalDataCookieName = @"_bu";
		/// <summary>
		/// Gets the name of the frontoffice user revival data cookie.
		/// </summary>
		public const string FrontofficeUserRevivalDataCookieName = @"_fu";
		/// <summary>
		/// The namespace in which the web tags live.
		/// </summary>
		public const string NamespaceUri = "http://schemas.premotion.nl/mansion/1.0/web/tags.xsd";
		/// <summary>
		/// The namespace in which the Web descriptors live.
		/// </summary>
		public const string DescriptorNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/web/descriptors.xsd";
		/// <summary>
		/// The namespace in which the control tags live.
		/// </summary>
		public const string ControlTagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/web.controls.tags.xsd";
		/// <summary>
		/// The namespace in which the data provider tags live.
		/// </summary>
		public const string DataProviderTagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/web.control.provider.tags.xsd";
		/// <summary>
		/// The namespace in which the form tags live.
		/// </summary>
		public const string FormTagNamespaceUri = @"http://schemas.premotion.nl/mansion/1.0/web.form.tags.xsd";
		/// <summary>
		/// The default backoffice script name.
		/// </summary>
		public const string DefaultBackofficeScriptName = "Cms/Cms.xts";
		/// <summary>
		/// The default frontoffice script name.
		/// </summary>
		public const string DefaultFrontofficeScriptName = "Default.xts";
		/// <summary>
		/// Defines the prefix for application content.
		/// </summary>
		public const string StaticContentPrefix = "application-content";
		/// <summary>
		/// Defines the prefix for streamed application content.
		/// </summary>
		public const string StreamingStaticContentPrefix = "stream-application-content";
		/// <summary>
		/// Defines the prefix for static applications resources.
		/// </summary>
		public const string StaticResourcesPrefix = "static-resources";
		/// <summary>
		/// Defines the prefix for dynamic applications resources.
		/// </summary>
		public const string DynamicResourcesPrefix = "dynamic-resources";
		/// <summary>
		/// Defines the prefix for merging applications resources.
		/// </summary>
		public const string MergeResourcesPrefix = "merge-resources";
	}
}