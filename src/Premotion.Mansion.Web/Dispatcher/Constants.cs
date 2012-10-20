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
		public const string RouteUrlPrefix = @"_route_";
		/// <summary>
		/// The prefix for route URL parameters.
		/// </summary>
		public const string RouteParameterPrefix = @"_";
		/// <summary>
		/// The namespace in which the dispatcher tags live.
		/// </summary>
		public const string TagNamespaceUri = "http://schemas.premotion.nl/mansion/1.0/web/dispatcher/tags.xsd";
		/// <summary>
		/// The charachters which will be trimmed from a url part.
		/// </summary>
		public static readonly char[] UrlPartTrimCharacters = new[] {'/', '\\'};
		/// <summary>
		/// The key of the Forwared from 404 header.
		/// </summary>
		public const string ForwardedFrom404HeaderKey = "Mansion-Forwarded-From-404";
	}
}