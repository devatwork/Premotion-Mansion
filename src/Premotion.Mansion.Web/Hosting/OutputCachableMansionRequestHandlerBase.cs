using System;
using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Base class for all <see cref="MansionRequestHandlerBase"/> which can be output cached.
	/// </summary>
	public abstract class OutputCachableMansionRequestHandlerBase : MansionRequestHandlerBase
	{
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="OutputCachableMansionRequestHandlerBase"/> with the given <paramref name="priority"/>.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="priority">The relative priority of this object. The higher the priority, earlier this object is executed.</param>
		/// <param name="specification">The specification which checks wether the current request can be handled by this handler.</param>
		protected OutputCachableMansionRequestHandlerBase(ICachingService cachingService, int priority, IRequestHandlerSpecification specification) : base(priority, specification)
		{
			// validate arguments
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values
			this.cachingService = cachingService;
		}
		#endregion
		#region Overrides of MansionRequestHandlerBase
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		protected override sealed void DoExecute(IMansionWebContext context)
		{
			// generate the cache key
			var cacheKey = OutputCacheRequestHandler.GenerateCacheKeyForRequest(context.HttpContext);

			// add the result to the 
			cachingService.AddOrReplace(context, cacheKey, () =>
			                                               {
			                                               	// create an web output pipe, push it to the stack and allow implementors to process the request on it
			                                               	using (var outputPipe = new WebOutputPipe(context.HttpContext)
			                                               	                        {
			                                               	                        	OutputCacheEnabled = true
			                                               	                        })
			                                               	{
			                                               		using (context.OutputPipeStack.Push(outputPipe))
			                                               			DoExecute(context, outputPipe);

			                                               		// create the cache container
			                                               		var cacheContainer = OutputCacheRequestHandler.CachableWebResponse.CreateCachedWebResponse(context, outputPipe);

			                                               		// do not cache requests other than GET request, check if the output pipe can be cached
			                                               		cacheContainer.IsCachable = OutputCacheRequestHandler.IsCachableRequest(context) && outputPipe.OutputCacheEnabled;

			                                               		// if the response is cacheable set the cache parameters
			                                               		if (cacheContainer.IsCachable)
			                                               			OutputCacheRequestHandler.SetCacheControlProperties(context.HttpContext.Response, cacheContainer.Object);

			                                               		// write the content to the response
			                                               		cacheContainer.Object.Flush(context.HttpContext.Response);

			                                               		return cacheContainer;
			                                               	}
			                                               });
		}
		#endregion
		#region Template Methods
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> to which the must should be written.</param>
		protected abstract void DoExecute(IMansionWebContext context, WebOutputPipe outputPipe);
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		#endregion
	}
}