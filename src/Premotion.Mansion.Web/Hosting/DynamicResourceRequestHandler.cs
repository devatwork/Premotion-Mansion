using System;
using System.Text;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="MansionRequestHandlerBase"/> for dynamic resources like CSS and JavaScript.
	/// </summary>
	public class DynamicResourceRequestHandler : OutputCachableMansionRequestHandlerBase
	{
		#region Constants
		/// <summary>
		/// Defines the prefix for dynamic applications resources.
		/// </summary>
		public const string Prefix = "dynamic-resources";
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <param name="scriptService">The <see cref="IExpressionScriptService"/>.</param>
		public DynamicResourceRequestHandler(ICachingService cachingService, IApplicationResourceService resourceService, IExpressionScriptService scriptService) : base(cachingService, 40, new UrlPrefixSpeficiation(Prefix))
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

			// if the resource does not exist, send a 404
			if (!resourceService.Exists(context, resourcePath))
			{
				// send 404
				context.HttpContext.Response.StatusCode = 404;
				context.HttpContext.Response.StatusDescription = "Not Found";
				return;
			}

			// parse the resource script
			var script = scriptService.Parse(context, resourceService.GetSingle(context, resourcePath));

			// execute the script and write the result back to the output pipe
			outputPipe.Writer.Write(script.Execute<string>(context));

			// set expires header age
			outputPipe.Expires = DateTime.Now.AddYears(1);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		private readonly IExpressionScriptService scriptService;
		#endregion
	}
}