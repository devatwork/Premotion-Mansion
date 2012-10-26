using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Represents an URL.
	/// </summary>
	public class Url
	{
		#region Constuctors
		/// <summary>
		/// Create using factory methods.
		/// </summary>
		private Url()
		{
		}
		#endregion
		#region Methods
		/// <summary>
		/// Clones the this <see cref="Url"/>.
		/// </summary>
		/// <returns>Returns the cloned <see cref="Url"/>.</returns>
		public Url Clone()
		{
			// create the clone
			var clone = new Url
			            {
			            	Scheme = Scheme,
			            	HostName = HostName,
			            	Port = Port,
			            	ApplicationPathSegments = ApplicationPathSegments.ToArray(),
			            	PathSegments = PathSegments.ToArray(),
			            	Fragment = Fragment
			            };

			// copy the query string
			foreach (var kvp in QueryString)
				clone.QueryString.Add(kvp);

			// return the clone
			return clone;
		}
		/// <summary>
		/// Creates a new <see cref="Url"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns the created <see cref="Url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public static Url CreateUrl(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the application url
			var applicationUrl = context.Request.ApplicationUrl;

			return new Url
			       {
			       	Scheme = applicationUrl.Scheme,
			       	HostName = applicationUrl.HostName,
			       	Port = applicationUrl.Port,
			       	ApplicationPathSegments = applicationUrl.ApplicationPathSegments,
			       	PathSegments = new string[0]
			       };
		}
		/// <summary>
		/// Parses the given <paramref name="uri"/> as an url.
		/// </summary>
		/// <param name="applicationUrl">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="uri">The <see cref="Uri"/> which to parse.</param>
		/// <returns>Returns the parsed <see cref="Url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="uri"/> is null.</exception>
		public static Url ParseUri(Url applicationUrl, Uri uri)
		{
			// validate arguments
			if (applicationUrl == null)
				throw new ArgumentNullException("applicationUrl");
			if (uri == null)
				throw new ArgumentNullException("uri");

			// create the url
			var url = new Url
			          {
			          	Scheme = applicationUrl.Scheme,
			          	HostName = applicationUrl.HostName,
			          	Port = applicationUrl.Port,
			          	ApplicationPathSegments = applicationUrl.ApplicationPathSegments,
			          	PathSegments = uri.Segments.Select(candidate => candidate.Trim(Dispatcher.Constants.UrlPartTrimCharacters)).Where(candidate => candidate.Length > 0).Skip(applicationUrl.ApplicationPathSegments.Length).ToArray(),
							CanHaveExtension = true
			          };

			// parse the query string
			var nvc = HttpUtility.ParseQueryString(uri.Query);
			foreach (var key in nvc.Cast<string>())
				url.QueryString[key] = nvc[key];

			// return the parsed url
			return url;
		}
		/// <summary>
		/// Parses the given <paramref name="uri"/> as the application url.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/> which to parse.</param>
		/// <returns>Returns the parsed <see cref="Url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="uri"/> is null.</exception>
		public static Url ParseApplicationUri(Uri uri)
		{
			// validate arguments
			if (uri == null)
				throw new ArgumentNullException("uri");

			// return the url
			return new Url
			       {
			       	Scheme = uri.Scheme,
			       	HostName = uri.Host,
			       	Port = uri.Port,
			       	ApplicationPathSegments = uri.Segments.Select(candidate => candidate.Trim(Dispatcher.Constants.UrlPartTrimCharacters)).Where(candidate => candidate.Length > 0).ToArray(),
			       	PathSegments = new string[0]
			       };
		}
		/// <summary>
		/// Parses the given <paramref name="relativeUrl"/> into a <see cref="Url"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="relativeUrl">The relative url, may contain path, query string and fragment.</param>
		/// <returns>Returns the parsed <see cref="Url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static Url ParseUrl(IMansionWebContext context, string relativeUrl)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (relativeUrl == null)
				throw new ArgumentNullException("relativeUrl");

			// begin by creating a copy of the application url
			var url = context.Request.ApplicationUrl.Clone();

			// tracks how far the end of the relative url has already been parsed
			var parsedIndex = relativeUrl.Length - 1;

			// check if the relative url contains a fragment
			var fragmentStart = relativeUrl.LastIndexOf('#', parsedIndex, parsedIndex);
			if (fragmentStart > -1)
			{
				url.Fragment = relativeUrl.Substring(fragmentStart);
				parsedIndex -= url.Fragment.Length;
				if (parsedIndex == 0)
					return url;
			}

			// check if the relative url contains a query string
			var queryStringStart = relativeUrl.LastIndexOf('?', parsedIndex, parsedIndex);
			if (queryStringStart > -1)
			{
				var queryString = relativeUrl.Substring(queryStringStart, parsedIndex - queryStringStart + 1);
				var nvc = HttpUtility.ParseQueryString(queryString);
				foreach (var key in nvc.Cast<string>())
					url.QueryString[key] = nvc[key];
				parsedIndex -= queryString.Length;
				if (parsedIndex == 0)
					return url;
			}

			// intepret the remainder as the path
			url.PathSegments = relativeUrl.Substring(0, parsedIndex + 1).Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// return the parsed url
			return url;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the HTTP protocol used by the client.
		/// </summary>
		/// <value>The protocol.</value>
		public string Scheme
		{
			get { return scheme.ToLower(); }
			set { scheme = (value ?? string.Empty).ToLower(); }
		}
		/// <summary>
		/// Gets or sets the host name.
		/// </summary>
		public string HostName { get; set; }
		/// <summary>
		/// Gets or sets the port name.
		/// </summary>
		public int? Port { get; set; }
		/// <summary>
		/// Gets the base path segments.
		/// </summary>
		private string[] ApplicationPathSegments { get; set; }
		/// <summary>
		/// Gets the path of the request, relative to the base path.
		/// 
		/// This property drives the route matching
		/// </summary>
		public string Path
		{
			get { return FormatPath(PathSegments).Trim(Dispatcher.Constants.UrlPartTrimCharacters); }
		}
		/// <summary>
		/// 
		/// </summary>
		public string Filename
		{
			get
			{
				// check if there is at least one segment
				if (PathSegments.Length == 0 || !CanHaveExtension)
					return string.Empty;

				// if the last segment contains a dot, it is a file name
				var lastSegment = PathSegments[PathSegments.Length - 1];
				var dotIndexOfLastSegment = lastSegment.LastIndexOf('.');
				return dotIndexOfLastSegment == -1 ? string.Empty : lastSegment.Trim(Dispatcher.Constants.UrlPartTrimCharacters);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BasePath
		{
			get
			{
				// check if there is at least one segment
				if (PathSegments.Length == 0)
					return string.Empty;

				// check if the last segment is a file name
				var basePathSegmentCount = PathSegments.Length - (string.IsNullOrEmpty(Filename) ? 0 : 1);

				// return the base path
				return FormatPath(PathSegments.Take(basePathSegmentCount)).Trim(Dispatcher.Constants.UrlPartTrimCharacters);
			}
		}
		/// <summary>
		/// Gets the segments of the path.
		/// </summary>
		public string[] PathSegments { get; set; }
		/// <summary>
		/// Gets the query string of this url.
		/// </summary>
		public IDictionary<string, string> QueryString
		{
			get { return queryString; }
		}
		/// <summary>
		/// Gets/Sets the fragment of this url.
		/// </summary>
		public string Fragment
		{
			get { return fragment.Trim('#'); }
			set { fragment = (value ?? string.Empty).Trim('#'); }
		}
		/// <summary>
		/// Gets/Sets a flag indicating wether this url can have an extension.
		/// </summary>
		public bool CanHaveExtension { get; set; }
		#endregion
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			// create a buffer
			return new StringBuilder()
				.Append(Scheme)
				.Append("://")
				.Append(FormatHostName(HostName))
				.Append(FormatPort(Port))
				.Append(FormatPath(ApplicationPathSegments))
				.Append(FormatPath(PathSegments))
				.Append(FormatQueryString(QueryString))
				.Append(FormatFragment(Fragment))
				.ToString();
		}
		#endregion
		#region Operators
		/// <summary>
		/// Converts the given <paramref name="url"/> into a <see cref="String"/>.
		/// </summary>
		/// <param name="url">The <see cref="Url"/>.</param>
		/// <returns>Returns the string representation of the <paramref name="url"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="url"/> is null.</exception>
		public static implicit operator string(Url url)
		{
			// validate arguments
			if (url == null)
				throw new ArgumentNullException("url");

			// return the string
			return url.ToString();
		}
		#endregion
		#region Helper Methods
		/// <summary>
		/// Gets the formatted host name.
		/// </summary>
		/// <param name="hostName"></param>
		/// <returns></returns>
		private static string FormatHostName(string hostName)
		{
			IPAddress address;

			if (IPAddress.TryParse(hostName, out address))
				return (address.AddressFamily == AddressFamily.InterNetworkV6) ? string.Concat("[", address.ToString(), "]") : address.ToString();

			return hostName;
		}
		/// <summary>
		/// Gets the formatted port number
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		private string FormatPort(int? port)
		{
			// check if a port is specified
			if (!port.HasValue)
				return string.Empty;

			if (port.Value == 80 && "http".Equals(Scheme))
				return string.Empty;

			if (port.Value == 443 && "https".Equals(Scheme))
				return string.Empty;

			return string.Concat(":", port.Value);
		}
		/// <summary>
		/// Formats the path.
		/// </summary>
		/// <param name="segments"></param>
		/// <returns></returns>
		private string FormatPath(IEnumerable<string> segments)
		{
			if (segments == null)
				throw new ArgumentNullException("segments");

			// decode all the segments
			var segmentArray = segments.Select(segment => segment.UrlDecode().HtmlDecode().Trim(' ').Trim(Dispatcher.Constants.UrlPartTrimCharacters)).ToArray();

			// url path encode the remaining segments
			segments = segmentArray.Select((segment, index) => segment.UrlPathEncode((index == segmentArray.Length - 1) && CanHaveExtension));

			// filter out empty segments
			segments = segments.Where(segment => !string.IsNullOrEmpty(segment));

			// assemble the path
			return segments.Aggregate(string.Empty, (current, part) => current + "/" + part);
		}
		/// <summary>
		/// Formats the given query string.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		private static string FormatQueryString(IDictionary<string, string> query)
		{
			return (query.Count == 0) ? string.Empty : "?" + string.Join("&", query.Select(kvp => kvp.Key.UrlEncode() + "=" + kvp.Value.UrlEncode()));
		}
		/// <summary>
		/// Formats the given fragment.
		/// </summary>
		/// <param name="fragment"></param>
		/// <returns></returns>
		private static string FormatFragment(string fragment)
		{
			return (string.IsNullOrEmpty(fragment)) ? string.Empty : string.Concat("#", fragment);
		}
		#endregion
		#region Private Fields
		private readonly Dictionary<string, string> queryString = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		private string fragment = string.Empty;
		private string scheme;
		#endregion
	}
}