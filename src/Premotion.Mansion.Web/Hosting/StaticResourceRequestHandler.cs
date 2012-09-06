using System;
using System.Text;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="MansionRequestHandlerBase"/> for static application resources like CSS and JavaScript.
	/// </summary>
	public class StaticResourceRequestHandler : OutputCachableMansionRequestHandlerBase
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for static applications resources.
		/// </summary>
		public const string Prefix = "static-resources";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		public StaticResourceRequestHandler(ICachingService cachingService, IApplicationResourceService resourceService) : base(cachingService, 50, new UrlPrefixSpeficiation(Prefix))
		{
			// validate arguments
			if (resourceService == null)
				throw new ArgumentNullException("resourceService");

			// set values
			this.resourceService = resourceService;
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
			var resourcePath = new RelativeResourcePath(originalResourcePath, false);

			// set output pipe properties
			outputPipe.ContentType = HttpUtilities.GetMimeType(originalResourcePath);
			outputPipe.Encoding = Encoding.UTF8;

			// if the resource exist process it otherwise 404
			if (!resourceService.Exists(context, resourcePath))
			{
				// send 404
				context.HttpContext.Response.StatusCode = 404;
				context.HttpContext.Response.StatusDescription = "Not Found";
				return;
			}

			// parse the resource script
			var resource = resourceService.GetSingle(context, resourcePath);

			// stream the file
			using (var reader = resource.OpenForReading())
				reader.RawStream.CopyTo(outputPipe.RawStream);

			// set expires header age
			outputPipe.Expires = DateTime.Now.AddYears(1);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		#endregion
	}
}