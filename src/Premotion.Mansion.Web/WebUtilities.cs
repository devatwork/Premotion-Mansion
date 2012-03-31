using System;
using System.Linq;
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
			var webOutputPipe = (WebOutputPipe) context.OutputPipeStack.FirstOrDefault(x => x is WebOutputPipe);
			if (webOutputPipe == null)
				throw new InvalidOperationException("No web output pipe was found on thet stack.");

			// disable the cache
			webOutputPipe.OutputCacheEnabled = false;
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
		/// <param name="url">The <see cref="Uri"/> to which to redirect.</param>
		/// <param name="permanent">Flag indicating whether the redirect is permanent or not.</param>
		public static void RedirectRequest(IMansionContext context, Uri url, bool permanent = false)
		{
			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// disable the caches
			DisableOutputCache(context);
			DisableResponseTemplateCache(context);

			// set redirect
			webContext.HttpContext.Response.RedirectLocation = url.ToString();
			webContext.HttpContext.Response.StatusCode = permanent ? 301 : 302;
			webContext.HttpContext.ApplicationInstance.CompleteRequest();

			// halt execution
			context.BreakExecution = true;
		}
		#endregion
		#region Uri Methods
		/// <summary>
		/// Strips the <see cref="UriComponents.Port"/> from the <paramref name="original"/>.
		/// </summary>
		/// <param name="original">The original <see cref="Uri"/>.</param>
		/// <returns>Returns the stripped <see cref="Uri"/>.</returns>
		public static Uri StripPort(Uri original)
		{
			// validate arguments
			if (original == null)
				throw new ArgumentNullException("original");

			return new Uri(original.GetComponents(UriComponents.Fragment | UriComponents.Query | UriComponents.Path | UriComponents.Host | UriComponents.UserInfo | UriComponents.Scheme, UriFormat.SafeUnescaped));
		}
		#endregion
	}
}