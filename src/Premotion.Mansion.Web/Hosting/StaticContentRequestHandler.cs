using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for user content like uploaded images and documents.
	/// </summary>
	public class StaticContentRequestHandler : WebOutputRequestHandler
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for application content.
		/// </summary>
		public const string Prefix = "application-content";
		#endregion
		#region Nested type: StaticContentRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class StaticContentRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="resourceService"></param>
			public StaticContentRequestHandlerFactory(IContentResourceService resourceService) : base(new UrlPrefixSpecification(Prefix))
			{
				// validate arguments
				if (resourceService == null)
					throw new ArgumentNullException("resourceService");

				// set values
				this.resourceService = resourceService;
			}
			#endregion
			#region Overrides of RequestHandlerFactory
			/// <summary>
			/// Constructs a <see cref="RequestHandler"/>.
			/// </summary>
			/// <param name="applicationContext">The <see cref="IMansionContext"/> of the application.</param>
			/// <returns>Returns the constructed <see cref="RequestHandler"/>.</returns>
			protected override RequestHandler DoCreate(IMansionContext applicationContext)
			{
				return new StaticContentRequestHandler(resourceService);
			}
			#endregion
			#region Private Fields
			private readonly IContentResourceService resourceService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="contentService">The <see cref="IContentResourceService"/>.</param>
		public StaticContentRequestHandler(IContentResourceService contentService)
		{
			// validate arguments
			if (contentService == null)
				throw new ArgumentNullException("contentService");

			// set values
			this.contentService = contentService;
		}
		#endregion
		#region Overrides of WebOutputRequestHandler
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> to which the must should be written.</param>
		protected override void DoExecute(IMansionWebContext context, WebOutputPipe outputPipe)
		{
			// retrieve the resource
			var originalResourcePath = context.Request.RequestUrl.Path.Substring(Prefix.Length + 1);

			// split the path
			var pathParts = originalResourcePath.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// parse the path
			var contentPath = contentService.ParsePath(context, new PropertyBag {
				{"category", pathParts[0]},
				{"relativePath", string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), pathParts)}
			});

			// set output pipe properties
			outputPipe.Response.ContentType = WebUtilities.GetMimeType(originalResourcePath);
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
				outputPipe.Response.CacheSettings.Expires = DateTime.Now.AddYears(1);
			}
			else
			{
				// send 404
				outputPipe.Response.StatusCode = HttpStatusCode.NotFound;
				outputPipe.Response.StatusDescription = "Not Found";
			}
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentService;
		#endregion
	}
}