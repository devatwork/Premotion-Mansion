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
			/// <param name="response">The <see cref="HttpResponseBase"/> being cached.</param>
			/// <param name="contentBytes">The content of <paramref name="response"/>.</param>
			private CachableWebResponse(HttpResponseBase response, byte[] contentBytes)
			{
				// validate arguments
				if (response == null)
					throw new ArgumentNullException("response");
				if (contentBytes == null)
					throw new ArgumentNullException("contentBytes");

				// copy values
				this.contentBytes = contentBytes;
				contentType = response.ContentType;
				contentEncoding = response.ContentEncoding;
				redirectLocation = response.RedirectLocation;
				statusCode = response.StatusCode;
				statusDescription = response.StatusDescription;
				headers = response.Headers;

				// assemble header string
				var headerStringBuilder = new StringBuilder();
				foreach (var name in headers.AllKeys)
					headerStringBuilder.AppendFormat("{0}={1}", name, headers[name]);

				// generate the etag
				var eTagString = contentBytes + contentType + contentEncoding.EncodingName + redirectLocation + statusCode + statusDescription + headerStringBuilder;
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
			#region Cache Methods
			/// <summary>
			/// Creates a cachable web response.
			/// </summary>
			/// <param name="response">The <see cref="HttpResponseBase"/> which is cached.</param>
			/// <param name="contentBytes">The content of <paramref name="response"/>.</param>
			/// <returns>Returns the created <see cref="CachedObject{TObject}"/>.</returns>
			public static CachedObject<CachableWebResponse> CreateCachedWebResponse(HttpResponseBase response, byte[] contentBytes)
			{
				// validate arguments
				if (response == null)
					throw new ArgumentNullException("response");
				if (contentBytes == null)
					throw new ArgumentNullException("contentBytes");

				// create the cachable web response
				var cachableResponse = new CachableWebResponse(response, contentBytes);

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
			cachingService = MansionHttpApplication.Nucleus.ResolveSingle<ICachingService>();

			// listen to begin request
			context.PostAcquireRequestState += PostAcquireRequestState;
		}
		#endregion
		#region Http Module Event Handlers
		/// <summary>
		/// Fired at the beginning of each request.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PostAcquireRequestState(object sender, EventArgs e)
		{
			// validate arguments
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (e == null)
				throw new ArgumentNullException("e");

			// get variables
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			var httpRequestContext = httpContext.Request;

			// Only get request can be cached because other types are state modifying
			if (!"GET".Equals(httpRequestContext.HttpMethod, StringComparison.OrdinalIgnoreCase))
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
			var context = ContextFactoryHttpModule.RequestContext;
			CachedObject<CachableWebResponse> cachedResponseContainer;
			if (!cachingService.TryGet(context, cacheKey, out cachedResponseContainer))
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
			return (StringCacheKey) string.Format("host={0}&port={1}&path={2}&get={3}&backoffice-user-authenticated={4}",
			                                      httpContext.Request["HTTP_HOST"],
			                                      httpContext.Request["SERVER_PORT"],
			                                      PathRewriterHttpModule.GetOriginalRawPath(httpContext),
			                                      httpContext.Request["QUERY_STRING"],
			                                      httpContext.HasSession() && (httpContext.Session[Constants.BackofficeUserRevivalDataCookieName] != null)
			                        	);
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
			response.Cache.SetLastModified(cachedResponse.LastModified);
			response.Cache.SetETag(cachedResponse.ETag);
		}
		#endregion
		#region Private Fields
		private ICachingService cachingService;
		#endregion
	}
}