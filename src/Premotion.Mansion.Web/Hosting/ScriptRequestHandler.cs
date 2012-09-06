using System;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="MansionRequestHandlerBase"/> for mansion script requests.
	/// </summary>
	public class ScriptRequestHandler : OutputCachableMansionRequestHandlerBase
	{
		#region Constants
		/// <summary>
		/// The default backoffice path.
		/// </summary>
		private static readonly RelativeResourcePath DefaultBackofficePath = new RelativeResourcePath(Constants.DefaultBackofficeScriptName, false);
		/// <summary>
		/// The default frontoffice path.
		/// </summary>
		private static readonly RelativeResourcePath DefaultFrontofficePath = new RelativeResourcePath(Constants.DefaultFrontofficeScriptName, false);
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <param name="scriptService">The <see cref="ITagScriptService"/>.</param>
		public ScriptRequestHandler(ICachingService cachingService, IApplicationResourceService resourceService, ITagScriptService scriptService) : base(cachingService, 10, AlwaysSatisfiedSpecification.Instance)
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
			// always disable cache for backoffice users
			if (context.BackofficeUserState.IsAuthenticated)
				outputPipe.OutputCacheEnabled = false;

			// get the application path
			var applicationPath = context.HttpContext.Request.ApplicationPath ?? string.Empty;

			// determine path to the script which to execute
			var scriptPath = new RelativeResourcePath(context.HttpContext.Request.Path.Substring(applicationPath.Length), false);

			// check if the request is to an actual script file, use the default script in that case
			if (!resourceService.Exists(context, scriptPath))
				scriptPath = context.IsBackoffice ? DefaultBackofficePath : DefaultFrontofficePath;

			// parse the script
			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, scriptPath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the minimal required <see cref="RequiresSessionState"/> for this handler.
		/// </summary>
		public override RequiresSessionState MinimalStateDemand
		{
			get { return RequiresSessionState.Full; }
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService resourceService;
		private readonly ITagScriptService scriptService;
		#endregion
	}
}