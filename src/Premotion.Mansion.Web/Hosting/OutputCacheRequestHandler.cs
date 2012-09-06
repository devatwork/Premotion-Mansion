using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements <see cref="OutputCacheRequestHandler"/> for output cache.
	/// </summary>
	public class OutputCacheRequestHandler : MansionRequestHandlerBase
	{
		#region Constants
		private const string CachedResponseKey = "cached-response";
		#endregion
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
				var originalPath = context.Request.GetPathWithoutHandlerPrefix();

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
		#region Nested type: OutputCachableRequestSpecification
		/// <summary>
		/// Checks whether the current request is already cached.
		/// </summary>
		private class OutputCachableRequestSpecification : IRequestHandlerSpecification
		{
			#region Constructors
			/// <summary>
			/// Constructs this specification.
			/// </summary>
			/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
			/// <exception cref="ArgumentNullException">Thrown if <paramref name="cachingService"/> is null.</exception>
			public OutputCachableRequestSpecification(ICachingService cachingService)
			{
				// validate arguments
				if (cachingService == null)
					throw new ArgumentNullException("cachingService");

				// set value
				this.cachingService = cachingService;
			}
			#endregion
			#region Implementation of ISpecification<in IMansionWebContext,out bool>
			/// <summary>
			/// Checks whether the given <paramref name="subject"/> satisfies this specification.
			/// </summary>
			/// <param name="subject">The subject which to check against this specification.</param>
			/// <returns>Returns the result of this check.</returns>
			public bool IsSatisfiedBy(IMansionWebContext subject)
			{
				var httpContext = subject.HttpContext;
				var httpRequestContext = httpContext.Request;

				// check if the request is not cachable
				if (!IsCachableRequest(subject))
					return false;

				// check if the browser requests a hard refresh
				if (httpRequestContext.Headers["Cache-Control"] != null && httpRequestContext.Headers["Cache-Control"].IndexOf("no-cache", StringComparison.OrdinalIgnoreCase) > -1)
					return false;

				// generate a cache key for this request
				var cacheKey = GenerateCacheKeyForRequest(httpContext);

				// try to get the cached response for this request
				CachedObject<CachableWebResponse> cachedResponseContainer;
				if (!cachingService.TryGet(subject, cacheKey, out cachedResponseContainer))
					return false;

				// store the cached response in the request, so we can use it later
				httpContext.Items[CachedResponseKey] = cachedResponseContainer.Object;

				// yes we can handle this request
				return true;
			}
			#endregion
			#region Private Fields
			private readonly ICachingService cachingService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs this output cache request handler.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		public OutputCacheRequestHandler(ICachingService cachingService) : base(100, new OutputCachableRequestSpecification(cachingService))
		{
		}
		#endregion
		#region Overrides of MansionRequestHandlerBase
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		protected override void DoExecute(IMansionWebContext context)
		{
			var httpContext = context.HttpContext;
			var httpRequestContext = httpContext.Request;

			// fetch the cached response from the request
			var cachedResponse = httpContext.Items[CachedResponseKey] as CachableWebResponse;
			if (cachedResponse == null)
				throw new InvalidOperationException("The cached web response was not found on a request marked as output cached");

			// make sure the request is cached properly by the browser
			var response = httpContext.Response;
			SetCacheControlProperties(response, cachedResponse);

			// check if the If-Modified-Since request header is not set
			DateTime modifiedSince;
			var ifModifiedSinceHeader = httpRequestContext.Headers["If-Modified-Since"];
			if (string.IsNullOrEmpty(ifModifiedSinceHeader) || !DateTime.TryParse(ifModifiedSinceHeader, out modifiedSince))
				modifiedSince = DateTime.MinValue;

			// check if the If-None-Match header request header is not set
			var eTag = httpRequestContext.Headers["If-None-Match"] ?? string.Empty;

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
			                                      httpContext.Request.Path,
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
			response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
			if (cachedResponse.Expires.HasValue)
				response.Cache.SetExpires(cachedResponse.Expires.Value);
			else
			{
				response.Cache.SetLastModified(cachedResponse.LastModified);
				response.Cache.SetETag(cachedResponse.ETag);
			}
		}
		#endregion
	}
}