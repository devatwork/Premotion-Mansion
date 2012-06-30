using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the <see cref="IHttpHandler"/> for static content like images and documents.
	/// </summary>
	public class StreamingStaticContentHttpHandler : MansionHttpHandlerBase, IReadOnlySessionState
	{
		#region Implementation of MansionHttpHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">Tjhe <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		protected override void ProcessRequest(IMansionWebContext context)
		{
			// create the request context
			var response = context.HttpContext.Response;

			// retrieve the resource
			var originalResourcePath = PathRewriterHttpModule.GetOriginalMappedPath(context.HttpContext);

			// split the path
			var pathParts = originalResourcePath.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// parse the path
			var contentService = context.Nucleus.ResolveSingle<IContentResourceService>();
			var contentPath = contentService.ParsePath(context, new PropertyBag
			                                                    {
			                                                    	{"category", pathParts[0]},
			                                                    	{"relativePath", string.Join(Path.DirectorySeparatorChar.ToString(), pathParts, 1, pathParts.Length - 1)}
			                                                    });

			// if the resource exist process it otherwise 404
			if (!contentService.Exists(context, contentPath))
			{
				// send 404
				response.StatusCode = 404;
				response.StatusDescription = "Not Found";
				return;
			}

			// get the resource
			var resource = contentService.GetResource(context, contentPath);
			var len = resource.Length;

			// set the headers
			response.Buffer = false;
			response.AppendHeader("Content-Length", len.ToString());
			response.ContentType = HttpUtilities.GetMimeType(originalResourcePath);

			// stream the file
			var buffer = new byte[1024];
			var outputStream = response.OutputStream;
			using (var reader = resource.OpenForReading())
			{
				int bytes;
				while (len > 0 && (bytes = reader.RawStream.Read(buffer, 0, buffer.Length)) > 0)
				{
					outputStream.Write(buffer, 0, bytes);
					len -= bytes;
				}
			}
		}
		#endregion
	}
}