using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the output cache for the current response.
	/// </summary>
	public class OutputCacheHttpModule : DisposableBase, IHttpModule
	{
		#region Nested type: CachableWebResponse
		/// <summary>
		/// Implements the cachable web response.
		/// </summary>
		public class CachableWebResponse
		{
			#region Constructors
			/// <summary>
			/// Constructs a cached web response.
			/// </summary>
			/// <param name="context">The <see cref="HttpContextBase"/> being cached.</param>
			/// <param name="contentBytes">The content of <paramref name="context"/>.</param>
			private CachableWebResponse(HttpContextBase context, byte[] contentBytes)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (contentBytes == null)
					throw new ArgumentNullException("contentBytes");

				// copy values
				this.contentBytes = contentBytes;
				contentType = context.Response.ContentType;
				contentEncoding = context.Response.ContentEncoding;
				redirectLocation = context.Response.RedirectLocation;
				statusCode = context.Response.StatusCode;
				statusDescription = context.Response.StatusDescription;
				headers = context.Response.Headers;

				// assemble header string
				var headerStringBuilder = new StringBuilder();
				foreach (var name in headers.AllKeys)
					headerStringBuilder.AppendFormat("{0}={1}", name, headers[name]);

				// get the original path
				var originalPath = PathRewriterHttpModule.GetOriginalMappedPath(context);

				// generate the etag
				var eTagString = originalPath + contentType + contentEncoding.EncodingName + redirectLocation + statusCode + statusDescription + headerStringBuilder;
				var eTagBytes = Encoding.UTF8.GetBytes(eTagString);
				byte[] eTagHash;
				using (var md5Service = new MD5CryptoServiceProvider())
					eTagHash = md5Service.ComputeHash(eTagBytes);
				eTag = Convert.ToBase64String(eTagHash);
			}
			#endregion
			#region Implementation of ICachableWebResponse
			/// <summary>
			/// Gets the date this response was last modified.
			/// </summary>
			public DateTime LastModified
			{
				get { return lastModified; }
			}
			/// <summary>
			/// Gets the ETag of this response.
			/// </summary>
			public string ETag
			{
				get { return eTag; }
			}
			/// <summary>
			/// Gets or sets the absolute date and time at which cached information expires in the cache.
			/// </summary>
			public DateTime? Expires { get; private set; }
			/// <summary>
			/// Flushes this response to the output.
			/// </summary>
			/// <param name="response"></param>
			public void Flush(HttpResponseBase response)
			{
				// validate arguments
				if (response == null)
					throw new ArgumentNullException("response");

				// copy the properties
				response.ContentEncoding = contentEncoding;
				response.ContentType = contentType;
				response.RedirectLocation = redirectLocation;
				response.StatusCode = statusCode;
				response.StatusDescription = statusDescription;

				// copy the headers
				foreach (var name in headers.AllKeys)
					response.AddHeader(name, headers[name]);

				// write the cached content
				response.OutputStream.Write(contentBytes, 0, contentBytes.Length);
			}
			#endregion
			#region Factory Methods
			/// <summary>
			/// Creates a cachable web response.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="outputPipe">The <see cref="WebOutputPipe"/> for which to create a cached object.</param>
			/// <returns>Returns the created <see cref="CachedObject{TObject}"/>.</returns>
			public static CachedObject<CachableWebResponse> CreateCachedWebResponse(IMansionWebContext context, WebOutputPipe outputPipe)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (outputPipe == null)
					throw new ArgumentNullException("outputPipe");

				// flush the response
				var contentBytes = outputPipe.Flush(context);

				// create the cachable web response
				var cachableResponse = new CachableWebResponse(context.HttpContext, contentBytes)
				                       {
				                       	Expires = outputPipe.Expires
				                       };

				// return the cache object
				return CachedObject<CachableWebResponse>.Create(cachableResponse);
			}
			#endregion
			#region Private Fields
			private readonly byte[] contentBytes;
			private readonly Encoding contentEncoding;
			private readonly string contentType;
			private readonly string eTag;
			private readonly NameValueCollection headers;
			private readonly DateTime lastModified = DateTime.Now;
			private readonly string redirectLocation;
			private readonly int statusCode;
			private readonly string statusDescription;
			#endregion
		}
		#endregion
		#region Implementation of IHttpModule
		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
		public void Init(HttpApplication context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the caching service reference
			cachingService = ContextFactoryHttpModule.Nucleus.ResolveSingle<ICachingService>();

			// listen to begin request
			context.PostAcquireRequestState += (sender, e) =>
			                                   {
			                                   	// get variables
			                                   	var webContext = ContextFactoryHttpModule.RequestContext.Cast<IMansionWebContext>();
			                                   	var httpContext = webContext.HttpContext;
			                                   	var httpRequestContext = httpContext.Request;

			                                   	// check if the request is not cachable
			                                   	if (!IsCachableRequest(webContext))
			                                   		return;

			                                   	// check if the browser requests a hard refresh
			                                   	if (httpRequestContext.Headers["Cache-Control"] != null && httpRequestContext.Headers["Cache-Control"].IndexOf("no-cache", StringComparison.OrdinalIgnoreCase) > -1)
			                                   		return;

			                                   	// check if the If-Modified-Since request header is not set
			                                   	DateTime modifiedSince;
			                                   	var ifModifiedSinceHeader = httpRequestContext.Headers["If-Modified-Since"];
			                                   	if (string.IsNullOrEmpty(ifModifiedSinceHeader) || !DateTime.TryParse(ifModifiedSinceHeader, out modifiedSince))
			                                   		return;

			                                   	// check if the If-None-Match header request header is not set
			                                   	var eTag = httpRequestContext.Headers["If-None-Match"];
			                                   	if (string.IsNullOrEmpty(eTag))
			                                   		return;

			                                   	// generate a cache key for this request
			                                   	var cacheKey = GenerateCacheKeyForRequest(httpContext);

			                                   	// try to get the cached response for this request
			                                   	CachedObject<CachableWebResponse> cachedResponseContainer;
			                                   	if (!cachingService.TryGet(webContext, cacheKey, out cachedResponseContainer))
			                                   		return;
			                                   	var cachedResponse = cachedResponseContainer.Object;

			                                   	// make sure the request is cached properly by the browser
			                                   	var response = httpContext.Response;
			                                   	SetCacheControlProperties(response, cachedResponse);

			                                   	// check if the ETag and LastModified date match
			                                   	if (eTag.Equals(cachedResponse.ETag, StringComparison.OrdinalIgnoreCase) && modifiedSince.AddSeconds(1) >= cachedResponse.LastModified)
			                                   	{
			                                   		// no changes have been made to the resource since last visit, return 304
			                                   		response.StatusCode = 304;
			                                   		response.StatusDescription = "Not Modified";
			                                   		response.AddHeader("Content-Length", "0"); //set to 0 to prevent client waiting for data
			                                   	}
			                                   	else
			                                   	{
			                                   		// flush the cached response to the browser
			                                   		cachedResponse.Flush(response);
			                                   	}

			                                   	// finish request, bypass all modules except EndRequest event
			                                   	httpContext.ApplicationInstance.CompleteRequest();
			                                   };
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
		}
		#endregion
		#region Cache Methods
		/// <summary>
		/// Generates a cache key for the request.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContextBase"/> for which to generate the request.</param>
		/// <returns>Returns the generated <see cref="CacheKey"/>.</returns>
		public static CacheKey GenerateCacheKeyForRequest(HttpContextBase httpContext)
		{
			// validate arugments
			if (httpContext == null)
				throw new ArgumentNullException("httpContext");

			// build de cache key
			return (StringCacheKey) string.Format("host={0}&port={1}&path={2}&get={3}",
			                                      httpContext.Request["HTTP_HOST"],
			                                      httpContext.Request["SERVER_PORT"],
			                                      PathRewriterHttpModule.GetOriginalRawPath(httpContext),
			                                      httpContext.Request["QUERY_STRING"]);
		}
		/// <summary>
		/// Gets a flag indicating wether this request is cachable.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <returns>Returns true when the request is cachable, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
		public static bool IsCachableRequest(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// never cache requests for backoffice request
			if (context.IsBackoffice)
				return false;

			// never cache requests for backoffice users
			if (context.BackofficeUserState.IsAuthenticated)
				return false;

			// only GET request can be cached
			return "GET".Equals(context.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Set the cache control properties.
		/// </summary>
		/// <param name="response">The <see cref="HttpResponseBase"/>.</param>
		/// <param name="cachedResponse">The <see cref="CachableWebResponse"/>.</param>
		public static void SetCacheControlProperties(HttpResponseBase response, CachableWebResponse cachedResponse)
		{
			// validate arguments
			if (response == null)
				throw new ArgumentNullException("response");
			if (cachedResponse == null)
				throw new ArgumentNullException("cachedResponse");

			// set cache control properties
			response.Cache.SetCacheability(HttpCacheability.Public);
			if (cachedResponse.Expires.HasValue)
				response.Cache.SetExpires(cachedResponse.Expires.Value);
			else
			{
				response.Cache.SetLastModified(cachedResponse.LastModified);
				response.Cache.SetETag(cachedResponse.ETag);
			}
		}
		#endregion
		#region Private Fields
		private ICachingService cachingService;
		#endregion
	}
}