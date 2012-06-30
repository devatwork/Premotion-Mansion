namespace Premotion.Mansion.Web.Dispatcher
{
	/// <summary>
	/// Defines constants for the dispatcher module.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The prefix for route URLs.
		/// </summary>
		public const string RouteUrlPrefix = @"-route-";
		/// <summary>
		/// The prefix for route URL parameters.
		/// </summary>
		public const string RouteParameterPrefix = @"-";
		/// <summary>
		/// The namespace in which the dispatcher tags live.
		/// </summary>
		public const string TagNamespaceUri = "http://schemas.premotion.nl/mansion/1.0/web/dispatcher/tags.xsd";
		/// <summary>
		/// The charachters which will be trimmed from a url part.
		/// </summary>
		public static readonly char[] UrlPartTrimCharacters = new[] {'/', '\\'};
	}
}