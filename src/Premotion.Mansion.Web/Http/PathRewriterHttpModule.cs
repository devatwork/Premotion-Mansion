using System;
using System.IO;
using System.Web;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the <see cref="IHttpModule"/> which re-writes the paths to resources. 
	/// </summary>
	public class PathRewriterHttpModule : DisposableBase, IHttpModule
	{
		#region Constants
		private static readonly string originalRawUrlKey = "OrginalUrl_Raw_" + Guid.NewGuid();
		private static readonly string originalRawPathKey = "OrginalPath_Raw_" + Guid.NewGuid();
		private static readonly string originalMappedPathKey = "OrginalPath_Mapped_" + Guid.NewGuid();
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

			// listen to begin request
			context.BeginRequest += BeginRequest;
		}
		#endregion
		#region Http Module Event Handlers
		/// <summary>
		/// Fired at the beginning of each request.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void BeginRequest(object sender, EventArgs e)
		{
			// validate arguments
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (e == null)
				throw new ArgumentNullException("e");

			// get variables
			var httpContext = new HttpContextWrapper(HttpContext.Current);

			// get the current path
			var currentPath = httpContext.Request.Path.Substring(httpContext.Request.ApplicationPath.Length);

			// store the original path
			httpContext.Items.Add(originalRawUrlKey, httpContext.Request.Url);
			httpContext.Items.Add(originalRawPathKey, httpContext.Request.Path);

			// if no extension is mapped route it to either the frontoffice or backoffice default
			var extension = Path.GetExtension(currentPath);
			if (string.IsNullOrEmpty(extension))
			{
				// make sure the path ends with a slash
				if (currentPath.Length == 0 || currentPath[currentPath.Length - 1] != '/')
					currentPath += '/';

				// get backoffice flag
				var isBackoffice = currentPath.StartsWith(@"/cms/", StringComparison.OrdinalIgnoreCase);

				// rewrite the path
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, isBackoffice ? Constants.DefaultBackofficeScriptName : Constants.DefaultFrontofficeScriptName));
				return;
			}

			// dont map script files
			if (Constants.ExecutableScriptExtension.Equals(extension, StringComparison.OrdinalIgnoreCase))
				return;

			// check the path
			var pathParts = currentPath.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
			if (pathParts.Length < 3)
				return;

			// try rewrite the requests
			//TODO: refactor this to lookup table
			if (Constants.StaticContentPrefix.Equals(pathParts[0], StringComparison.OrdinalIgnoreCase))
			{
				// rewrite to dynamic resource
				httpContext.Items.Add(originalMappedPathKey, String.Join("/", pathParts, 1, pathParts.Length - 1));
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, "/Static.Content"));
			}
			else if (Constants.StreamingStaticContentPrefix.Equals(pathParts[0], StringComparison.OrdinalIgnoreCase))
			{
				// rewrite to dynamic resource
				httpContext.Items.Add(originalMappedPathKey, String.Join("/", pathParts, 1, pathParts.Length - 1));
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, "/Streaming.Static.Content"));
			}
			else if (Constants.StaticResourcesPrefix.Equals(pathParts[0], StringComparison.OrdinalIgnoreCase))
			{
				// rewrite to dynamic resource
				httpContext.Items.Add(originalMappedPathKey, String.Join("/", pathParts, 1, pathParts.Length - 1));
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, "/Static.Application.Resource"));
			}
			else if (Constants.DynamicResourcesPrefix.Equals(pathParts[0], StringComparison.OrdinalIgnoreCase))
			{
				// rewrite to dynamic resource
				httpContext.Items.Add(originalMappedPathKey, String.Join("/", pathParts, 1, pathParts.Length - 1));
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, "/Dynamic.Application.Resource"));
			}
			else if (Constants.MergeResourcesPrefix.Equals(pathParts[0], StringComparison.OrdinalIgnoreCase))
			{
				// rewrite to merge resource
				httpContext.Items.Add(originalMappedPathKey, String.Join("/", pathParts, 1, pathParts.Length - 1));
				httpContext.RewritePath(HttpUtilities.CombineIntoRelativeUrl(httpContext.Request.ApplicationPath, "/Merge.Application.Resource"));
			}
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
			// check for unmanaged disposal
			if (!disposeManagedResources)
				return;
		}
		#endregion
		#region Original Path Methods
		/// <summary>
		/// Gets the original not rewritten <see cref="Uri"/> of the current <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="HttpContextBase"/> of the current request.</param>
		/// <returns>Returns the orginal <see cref="Uri"/>.</returns>
		public static Uri GetOriginalRawUrl(HttpContextBase context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the request was not rewritten, in that case return the current request path which is original
			var originalRawUrl = context.Items[originalRawUrlKey] as Uri;
			return originalRawUrl ?? context.Request.Url;
		}
		/// <summary>
		/// Gets the original not rewritten path of the current <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="HttpContextBase"/> of the current request.</param>
		/// <returns>Returns the orginal path.</returns>
		public static string GetOriginalRawPath(HttpContextBase context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the request was not rewritten, in that case return the current request path which is original
			var originalPath = context.Items[originalRawPathKey] as string;
			if (originalPath == null)
				return context.Request.Path;

			// return the original path
			return originalPath;
		}
		/// <summary>
		/// Gets the original not rewritten path of the current <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="HttpContextBase"/> of the current request.</param>
		/// <returns>Returns the orginal path.</returns>
		/// <exception cref="InvalidOperationException">Thrown when </exception>
		public static string GetOriginalMappedPath(HttpContextBase context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the request was not rewritten, in that case return the current request path which is original
			var originalPath = context.Items[originalMappedPathKey] as string;
			if (originalPath == null)
				throw new InvalidOperationException("This request has not been redirected");

			// return the)
			return originalPath;
		}
		#endregion
	}
}