using System;
using System.Globalization;
using System.IO;
using System.Net;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for streaming static content like images and documents.
	/// </summary>
	public class StreamingStaticContentRequestHandler : RequestHandler
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for streamed application content.
		/// </summary>
		public const string Prefix = "streaming-application-content";
		#endregion
		#region Nested type: StreamingStaticContentRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class StreamingStaticContentRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="resourceService"></param>
			public StreamingStaticContentRequestHandlerFactory(IContentResourceService resourceService) : base(new UrlPrefixSpecification(Prefix))
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
				return new StreamingStaticContentRequestHandler(resourceService);
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
		public StreamingStaticContentRequestHandler(IContentResourceService contentService)
		{
			// validate arguments
			if (contentService == null)
				throw new ArgumentNullException("contentService");

			// set values
			this.contentService = contentService;
		}
		#endregion
		#region Overrides of MansionRequestHandlerBase
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <returns>Returns the <see cref="WebResponse"/>.</returns>
		protected override WebResponse DoExecute(IMansionWebContext context)
		{
			// create a new response
			var response = WebResponse.Create(context);

			// retrieve the resource
			var originalResourcePath = context.Request.RequestUrl.Path.Substring(Prefix.Length + 1);

			// split the path
			var pathParts = originalResourcePath.Split(Dispatcher.Constants.UrlPartTrimCharacters, StringSplitOptions.RemoveEmptyEntries);

			// parse the path
			var contentPath = contentService.ParsePath(context, new PropertyBag
			                                                    {
			                                                    	{"category", pathParts[0]},
			                                                    	{"relativePath", string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), pathParts, 1, pathParts.Length - 1)}
			                                                    });

			// if the resource exist process it otherwise 404
			if (!contentService.Exists(context, contentPath))
			{
				// send 404
				response.StatusCode = HttpStatusCode.NotFound;
				response.StatusDescription = "Not Found";
				return response;
			}

			// get the resource
			var resource = contentService.GetResource(context, contentPath);
			var len = resource.Length;

			// set the headers
			response.Headers.Add("Content-Length", len.ToString(CultureInfo.InvariantCulture));
			response.ContentType = HttpUtilities.GetMimeType(originalResourcePath);

			// set the content response
			response.Contents = stream =>
			                    {
			                    	// stream the file
			                    	var buffer = new byte[1024];
			                    	using (var reader = resource.OpenForReading())
			                    	{
			                    		int bytes;
			                    		while (len > 0 && (bytes = reader.RawStream.Read(buffer, 0, buffer.Length)) > 0)
			                    		{
			                    			stream.Write(buffer, 0, bytes);
			                    			len -= bytes;
			                    		}
			                    	}
			                    };

			// return the response
			return response;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the minimal required <see cref="RequiresSessionState"/> for this handler.
		/// </summary>
		public override RequiresSessionState MinimalSessionStateDemand
		{
			get { return RequiresSessionState.ReadOnly; }
		}
		#endregion
		#region Private Fields
		private readonly IContentResourceService contentService;
		#endregion
	}
}