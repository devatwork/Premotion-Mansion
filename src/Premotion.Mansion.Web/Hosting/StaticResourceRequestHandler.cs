using System;
using System.Net;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for static application resources like CSS and JavaScript.
	/// </summary>
	public class StaticResourceRequestHandler : WebOutputRequestHandler
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for static applications resources.
		/// </summary>
		public const string Prefix = "static-resources";
		#endregion
		#region Nested type: StaticResourceRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class StaticResourceRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="resourceService"></param>
			public StaticResourceRequestHandlerFactory(IApplicationResourceService resourceService) : base(new UrlPrefixSpecification(Prefix))
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
				return new StaticResourceRequestHandler(resourceService);
			}
			#endregion
			#region Private Fields
			private readonly IApplicationResourceService resourceService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		public StaticResourceRequestHandler(IApplicationResourceService resourceService)
		{
			// validate arguments
			if (resourceService == null)
				throw new ArgumentNullException("resourceService");

			// set values
			this.resourceService = resourceService;
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
			var resourcePath = new RelativeResourcePath(originalResourcePath, false);

			// set output pipe properties
			outputPipe.Response.ContentType = WebUtilities.GetMimeType(originalResourcePath);
			outputPipe.Encoding = Encoding.UTF8;

			// if the resource exist process it otherwise 404
			if (!resourceService.Exists(context, resourcePath))
			{
				// send 404
				outputPipe.Response.StatusCode = HttpStatusCode.NotFound;
				outputPipe.Response.StatusDescription = "Not Found";
				return;
			}

			// parse the resource script
			var resource = resourceService.GetSingle(context, resourcePath);

			// stream the file
			using (var reader = resource.OpenForReading())
				reader.RawStream.CopyTo(outputPipe.RawStream);

			// set expires header age
			outputPipe.Response.CacheSettings.Expires = DateTime.Now.AddYears(1);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		#endregion
	}
}