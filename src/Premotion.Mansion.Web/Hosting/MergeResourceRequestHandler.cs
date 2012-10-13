using System;
using System.Net;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for dynamic resources like CSS and JavaScript.
	/// </summary>
	public class MergeResourceRequestHandler : WebOutputRequestHandler
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for merging applications resources.
		/// </summary>
		public const string Prefix = "merge-resources";
		#endregion
		#region Nested type: MergeResourceRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class MergeResourceRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="resourceService"></param>
			/// <param name="scriptService"></param>
			public MergeResourceRequestHandlerFactory(IApplicationResourceService resourceService, IExpressionScriptService scriptService) : base(new UrlPrefixSpecification(Prefix))
			{
				// validate arguments
				if (resourceService == null)
					throw new ArgumentNullException("resourceService");
				if (scriptService == null)
					throw new ArgumentNullException("scriptService");

				// set values
				this.resourceService = resourceService;
				this.scriptService = scriptService;
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
				return new MergeResourceRequestHandler(resourceService, scriptService);
			}
			#endregion
			#region Private Fields
			private readonly IApplicationResourceService resourceService;
			private readonly IExpressionScriptService scriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <param name="scriptService">The <see cref="IExpressionScriptService"/>.</param>
		public MergeResourceRequestHandler(IApplicationResourceService resourceService, IExpressionScriptService scriptService)
		{
			// validate arguments
			if (resourceService == null)
				throw new ArgumentNullException("resourceService");
			if (scriptService == null)
				throw new ArgumentNullException("scriptService");

			// set values
			this.resourceService = resourceService;
			this.scriptService = scriptService;
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
			var originalResourcePath = context.HttpContext.Request.GetPathWithoutHandlerPrefix();
			var resourcePath = new RelativeResourcePath(originalResourcePath, true);

			// set output pipe properties
			outputPipe.Response.ContentType = HttpUtilities.GetMimeType(originalResourcePath);
			outputPipe.Encoding = Encoding.UTF8;

			// if the resource does not exist, send a 404
			if (!resourceService.Exists(context, resourcePath))
			{
				// send 404
				outputPipe.Response.StatusCode = HttpStatusCode.NotFound;
				outputPipe.Response.StatusDescription = "Not Found";
				return;
			}

			// merge all the resources
			foreach (var resource in resourceService.Get(context, resourcePath))
			{
				// parse the resource script
				var script = scriptService.Parse(context, resource);

				// execute the script and write the result back to the output pipe
				outputPipe.Writer.Write(script.Execute<string>(context));
			}

			// set expires header age
			outputPipe.Response.CacheSettings.Expires = DateTime.Now.AddYears(1);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		private readonly IExpressionScriptService scriptService;
		#endregion
	}
}