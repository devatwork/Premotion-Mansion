using System;
using System.Globalization;
using System.IO;
using System.Text;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="MansionRequestHandlerBase"/> for user content like uploaded images and documents.
	/// </summary>
	public class StaticContentRequestHandler : OutputCachableMansionRequestHandlerBase
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for application content.
		/// </summary>
		public const string Prefix = "application-content";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="contentService">The <see cref="IContentResourceService"/>.</param>
		public StaticContentRequestHandler(ICachingService cachingService, IContentResourceService contentService) : base(cachingService, 60, new UrlPrefixSpeficiation(Prefix))
		{
			// validate arguments
			if (contentService == null)
				throw new ArgumentNullException("contentService");

			// set values
			this.contentService = contentService;
		}
		#endregion
		#region Overrides of OutputCachableMansionRequestHandlerBase
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> to which the must should be written.</param>
		protected override void DoExecute(IMansionWebContext context, WebOutputPipe outputPipe)
		{
			// retrieve the resource
			var originalResourcePath = context.HttpContext.Request.GetPathWithoutHandlerPrefix();

			// split the path
			var pathParts = originalResourcePath.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// parse the path
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
				outputPipe.Expires = DateTime.Now.AddYears(1);
			}
			else
			{
				// send 404
				context.HttpContext.Response.StatusCode = 404;
				context.HttpContext.Response.StatusDescription = "Not Found";
			}
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentService;
		#endregion
	}
}