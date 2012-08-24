using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the <see cref="IHttpHandler"/> for mansion script requests.
	/// </summary>
	public class ScriptHttpHandler : OutputCachableHttpHandlerBase, IRequiresSessionState
	{
		#region Constants
		/// <summary>
		/// The default backoffice path.
		/// </summary>
		private static readonly RelativeResourcePath defaultBackofficePath = new RelativeResourcePath(Constants.DefaultBackofficeScriptName, false);
		/// <summary>
		/// The default frontoffice path.
		/// </summary>
		private static readonly RelativeResourcePath defaultFrontofficePath = new RelativeResourcePath(Constants.DefaultFrontofficeScriptName, false);
		#endregion
		#region Implementation of MansionHttpOutputCachableHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> constructed to which the response is written.</param>
		protected override void ProcessRequest(IMansionWebContext context, WebOutputPipe outputPipe)
		{
			// always disable cache for backoffice users
			if (context.BackofficeUserState.IsAuthenticated)
				outputPipe.OutputCacheEnabled = false;

			// determine path to the script which to execute
			var scriptPath = new RelativeResourcePath(context.HttpContext.Request.Path.Substring(context.HttpContext.Request.ApplicationPath.Length), false);

			// check if the request is to an actual script file, use the default script in that case
			var resourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			if (!resourceService.Exists(context, scriptPath))
				scriptPath = context.IsBackoffice ? defaultBackofficePath : defaultFrontofficePath;

			// parse the script
			var scriptService = context.Nucleus.ResolveSingle<ITagScriptService>();
			using (var script = scriptService.Parse(context, resourceService.GetSingle(context, scriptPath)))
			{
				script.Initialize(context);
				script.Execute(context);
			}
		}
		#endregion
	}
}