using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for mansion script requests.
	/// </summary>
	public class ScriptRequestHandler : WebOutputRequestHandler
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
		#region Nested type: ScriptRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class ScriptRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			/// <param name="resourceService"></param>
			/// <param name="scriptService"></param>
			public ScriptRequestHandlerFactory(IApplicationResourceService resourceService, ITagScriptService scriptService) : base(new ExtensionWhiteListSpecification(Constants.ScriptExtensions, true))
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
				return new ScriptRequestHandler(resourceService, scriptService);
			}
			#endregion
			#region Private Fields
			private readonly IApplicationResourceService resourceService;
			private readonly ITagScriptService scriptService;
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a <see cref="DynamicResourceRequestHandler"/>.
		/// </summary>
		/// <param name="resourceService">The <see cref="IApplicationResourceService"/>.</param>
		/// <param name="scriptService">The <see cref="ITagScriptService"/>.</param>
		public ScriptRequestHandler(IApplicationResourceService resourceService, ITagScriptService scriptService)
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
			// always disable cache for backoffice users
			if (context.BackofficeUserState.IsAuthenticated)
				outputPipe.Response.CacheSettings.OutputCacheEnabled = false;

			// determine path to the script which to execute
			var scriptPath = new RelativeResourcePath(context.Request.RequestUrl.Path, false);

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
		public override RequiresSessionState MinimalSessionStateDemand
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