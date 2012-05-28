using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the HTTP output caching features of an Mansion <see cref="IHttpHandler"/>.
	/// </summary>
	public abstract class OutputCachableHttpHandlerBase : MansionHttpHandlerBase
	{
		#region Implementation of MansionHttpHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		protected override sealed void ProcessRequest(IMansionWebContext context)
		{
			// generate the cache key
			var cacheKey = OutputCacheHttpModule.GenerateCacheKeyForRequest(context.HttpContext);

			// get the caching service
			var cachingService = context.Nucleus.ResolveSingle<ICachingService>();

			// add the result to the 
			cachingService.AddOrReplace(context, cacheKey, () =>
			                                               {
			                                               	// create an web output pipe, push it to the stack and allow implementors to process the request on it
			                                               	using (var outputPipe = new WebOutputPipe(context.HttpContext))
			                                               	{
			                                               		using (context.OutputPipeStack.Push(outputPipe))
			                                               			ProcessRequest(context, outputPipe);

			                                               		// flush the response
			                                               		var contentBytes = outputPipe.Flush(context);

			                                               		// create the cache container
			                                               		var cacheContainer = OutputCacheHttpModule.CachableWebResponse.CreateCachedWebResponse(context.HttpContext.Response, contentBytes);

			                                               		// do not cache requests other than GET request, check if the output pipe can be cached
			                                               		cacheContainer.IsCachable = OutputCacheHttpModule.IsCachableRequest(context) && outputPipe.OutputCacheEnabled;

			                                               		// if the response is cacheable set the cache parameters
			                                               		if (cacheContainer.IsCachable)
			                                               			OutputCacheHttpModule.SetCacheControlProperties(context.HttpContext.Response, cacheContainer.Object);

			                                               		// write the content to the response
			                                               		context.HttpContext.Response.OutputStream.Write(contentBytes, 0, contentBytes.Length);

			                                               		return cacheContainer;
			                                               	}
			                                               });
		}
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> constructed to which the response is written.</param>
		protected abstract void ProcessRequest(IMansionWebContext context, WebOutputPipe outputPipe);
		#endregion
	}
}