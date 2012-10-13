using System;
using System.Net;
using System.Web;
using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Web.Hosting.Caching
{
	/// <summary>
	/// Adds output caching to <see cref="WebOutputRequestHandler"/>s.
	/// </summary>
	public class WebOutputCacheRequestHandlerConfigurator : RequestHandlerConfigurator<WebOutputRequestHandler>
	{
		#region Constructors
		/// <summary></summary>
		/// <param name="cachingService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public WebOutputCacheRequestHandlerConfigurator(ICachingService cachingService)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// sets the caching service
			this.cachingService = cachingService;
		}
		#endregion
		#region Overrides of RequestHandlerConfigurator<WebOutputRequestHandler>
		/// <summary>
		/// Allows the configuration of the given <paramref name="handler"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="handler">The <see cref="RequestHandler"/> selected to handle the request.</param>
		protected override void DoConfigure(IMansionWebContext context, WebOutputRequestHandler handler)
		{
			// check if the request doest not allow for caching
			if (!IsCachableRequest(context))
				return;

			// generate the cache key
			var requestCacheKey = GenerateCacheKeyForRequest(context.HttpContext);

			// add a hook which checks for response already in cache
			handler.BeforePipeline.AddStageToBeginOfPipeline(ctx =>
			                                                 {
			                                                 	// get the context
			                                                 	var httpRequest = ctx.HttpContext.Request;

			                                                 	// check if the browser requests a hard refresh
			                                                 	if (httpRequest.Headers["Cache-Control"] != null && httpRequest.Headers["Cache-Control"].IndexOf("no-cache", StringComparison.OrdinalIgnoreCase) > -1)
			                                                 		return null;

			                                                 	// check if the response is not cached
			                                                 	CachedObject<WebResponse> cachedResponseContainer;
			                                                 	if (!cachingService.TryGet(ctx, requestCacheKey, out cachedResponseContainer))
			                                                 		return null;

			                                                 	// get the cached response
			                                                 	var response = cachedResponseContainer.Object.Clone();

			                                                 	// check if the If-Modified-Since request header is not set
			                                                 	DateTime modifiedSince;
			                                                 	var ifModifiedSinceHeader = httpRequest.Headers["If-Modified-Since"];
			                                                 	if (string.IsNullOrEmpty(ifModifiedSinceHeader) || !DateTime.TryParse(ifModifiedSinceHeader, out modifiedSince))
			                                                 		modifiedSince = DateTime.MinValue;

			                                                 	// check if the If-None-Match header request header is not set
			                                                 	var eTag = httpRequest.Headers["If-None-Match"] ?? string.Empty;

			                                                 	// check if the ETag and LastModified date match
			                                                 	if (eTag.Equals(response.CacheSettings.ETag, StringComparison.OrdinalIgnoreCase) && modifiedSince.AddSeconds(1) >= response.CacheSettings.LastModified)
			                                                 	{
			                                                 		// no changes have been made to the resource since last visit, return 304
			                                                 		response.StatusCode = HttpStatusCode.NotModified;
			                                                 		response.StatusDescription = "Not Modified";
			                                                 		response.Headers.Add("Content-Length", "0"); //set to 0 to prevent client waiting for data
			                                                 	}

			                                                 	// we handled the request
			                                                 	return response;
			                                                 });

			// add a hook which store responses which can be cached
			handler.AfterPipeline.AddStageToEndOfPipeline((ctx, response) =>
			                                              {
			                                              	// check if the response can not be cached
			                                              	if (!response.CacheSettings.OutputCacheEnabled)
			                                              		return;

			                                              	// clone the response
			                                              	var clonedResponse = response.Clone();

			                                              	// cache the cloned response
			                                              	cachingService.AddOrReplace(ctx, requestCacheKey, () =>
			                                              	                                                  {
			                                              	                                                  	// create the container
			                                              	                                                  	var container = new CachedObject<WebResponse>(clonedResponse);

			                                              	                                                  	// check if there is an expiration date
			                                              	                                                  	if (clonedResponse.CacheSettings.Expires.HasValue)
			                                              	                                                  		container.Add(new TimeSpanDependency(clonedResponse.CacheSettings.Expires.Value - clonedResponse.CacheSettings.LastModified));

			                                              	                                                  	// return the container
			                                              	                                                  	return container;
			                                              	                                                  });
			                                              });
		}
		#endregion
		#region Cache Methods
		/// <summary>
		/// Generates a cache key for the request.
		/// </summary>
		/// <param name="httpContext">The <see cref="HttpContextBase"/> for which to generate the request.</param>
		/// <returns>Returns the generated <see cref="CacheKey"/>.</returns>
		private static CacheKey GenerateCacheKeyForRequest(HttpContextBase httpContext)
		{
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
		private static bool IsCachableRequest(IMansionWebContext context)
		{
			// never cache requests for backoffice request
			if (context.IsBackoffice)
				return false;

			// only GET request can be cached
			return "GET".Equals(context.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}