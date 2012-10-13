using System;
using System.Linq;
using System.Net;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.ScriptTags.Rendering;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Provides utility methods for web applications.
	/// </summary>
	public static class WebUtilities
	{
		#region Cache Control Methods
		/// <summary>
		/// Disables the output cache of the current request.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		public static void DisableOutputCache(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the output pipe
			var webOutputPipe = context.GetWebOuputPipe();

			// disable the cache
			webOutputPipe.Response.CacheSettings.OutputCacheEnabled = false;
		}
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		public static void DisableResponseTemplateCache(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// find all the response template caches and disable their caches
			foreach (var responsePipe in context.OutputPipeStack.OfType<ResponseTemplateTag.ResponseOutputPipe>())
				responsePipe.ResponseCacheEnabled = false;
		}
		#endregion
		#region RedirectRequest Methods
		/// <summary>
		/// Redirects the request to the target <paramref name="url"/>. Disables the response and output caches and halts the script execution.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="url">The <see cref="Url"/> to which to redirect.</param>
		/// <param name="permanent">Flag indicating whether the redirect is permanent or not.</param>
		public static void RedirectRequest(IMansionContext context, Url url, bool permanent = false)
		{
			// get the output pipe
			var webOutputPipe = context.GetWebOuputPipe();

			// disable the caches
			DisableOutputCache(context);
			DisableResponseTemplateCache(context);

			// set redirect
			webOutputPipe.Response.RedirectLocation = url;
			webOutputPipe.Response.StatusCode = permanent ? HttpStatusCode.MovedPermanently : HttpStatusCode.TemporaryRedirect;

			// halt execution
			context.BreakExecution = true;
		}
		#endregion
	}
}