using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Wrapper around <see cref="HttpContext"/>.
	/// </summary>
	public interface IHttpContext
	{
		/// <summary>
		/// Gets the request object.
		/// </summary>
		IHttpRequest Request { get; }
		/// <summary>
		/// Gets the response object.
		/// </summary>
		IHttpResponse Response { get; }
		/// <summary>
		/// Gets the Cache object for the current application domain.
		/// </summary>
		Cache Cache { get; }
		/// <summary>
		/// Gets the HttpServerUtility object that provides methods used in processing Web requests.
		/// </summary>
		HttpServerUtility Server { get; }
		/// <summary>
		/// Gets a flag indicating whether there is a session for this request.
		/// </summary>
		bool HasSession { get; }
		/// <summary>
		/// Gets the HttpSessionState object for the current HTTP request.
		/// </summary>
		IHttpSessionState Session { get; }
		/// <summary>
		/// Gets or sets the HttpApplication object for the current HTTP request.
		/// </summary>
		HttpApplication ApplicationInstance { get; }
		/// <summary>
		/// Gets the TraceContext object for the current HTTP response.
		/// </summary>
		TraceContext Trace { get; }
		/// <summary>
		/// Gets a key/value collection that can be used to organize and share data between an <see cref="IHttpModule"/> interface and an <see cref="IHttpHandler"/> interface during an HTTP request.
		/// </summary>
		IDictionary Items { get; }
		/// <summary>
		/// Rewrites the URL using the given path.
		/// </summary>
		/// <param name="path">The internal rewrite path.</param>
		void RewritePath(string path);
	}
}