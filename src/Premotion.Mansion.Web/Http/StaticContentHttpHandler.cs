using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the <see cref="IHttpHandler"/> for static application resources like CSS and JavaScript.
	/// </summary>
	public class StaticContentHttpHandler : OutputCachableHttpHandlerBase, IReadOnlySessionState
	{
		#region Implementation of MansionHttpOutputCachableHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> constructed to which the response is written.</param>
		protected override void ProcessRequest(IMansionWebContext context, WebOutputPipe outputPipe)
		{
			// retrieve the resource
			var originalResourcePath = PathRewriterHttpModule.GetOriginalMappedPath(context.HttpContext);

			// split the path
			var pathParts = originalResourcePath.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// parse the path
			var contentService = context.Nucleus.ResolveSingle<IContentResourceService>();
			var contentPath = contentService.ParsePath(context, new PropertyBag
			                                                    {
			                                                    	{"category", pathParts[0]},
			                                                    	{"relativePath", string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), pathParts, 1, pathParts.Length - 1)}
			                                                    });

			// set output pipe properties
			outputPipe.ContentType = HttpUtilities.GetMimeType(originalResourcePath);
			outputPipe.Encoding = Encoding.UTF8;

			// if the resource exist process it otherwise 404
			if (contentService.Exists(context, contentPath))
			{
				// parse the resource script
				var resource = contentService.GetResource(context, contentPath);
				var len = resource.Length;

				// stream the file
				var buffer = new byte[1024];
				using (var reader = resource.OpenForReading())
				{
					int bytes;
					while (len > 0 && (bytes = reader.RawStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						outputPipe.RawStream.Write(buffer, 0, bytes);
						len -= bytes;
					}
				}

				// set cache age
				context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
				context.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
			}
			else
			{
				// send 404
				context.HttpContext.Response.StatusCode = 404;
				context.HttpContext.Response.StatusDescription = "Not Found";
			}
		}
		#endregion
	}
}