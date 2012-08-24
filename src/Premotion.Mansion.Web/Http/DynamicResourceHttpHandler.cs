﻿using System.Text;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Http
{
	/// <summary>
	/// Implements the <see cref="IHttpHandler"/> for dynamic resources like CSS and JavaScript.
	/// </summary>
	public class DynamicResourceHttpHandler : OutputCachableHttpHandlerBase, IReadOnlySessionState
	{
		#region Implementation of MansionHttpOutputCachableHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> constructed for handling the current request.</param>
		/// <param name="outputPipe">The <see cref="WebOutputPipe"/> constructed to which the response is written.</param>
		protected override void ProcessRequest(IMansionWebContext context, WebOutputPipe outputPipe)
		{
			// retrieve the resource
			var originalResourcePath = context.HttpContext.Request.GetPathWithoutHandlerPrefix();
			var resourcePath = new RelativeResourcePath(originalResourcePath, false);

			// set output pipe properties
			outputPipe.ContentType = HttpUtilities.GetMimeType(originalResourcePath);
			outputPipe.Encoding = Encoding.UTF8;

			// if the resource exist process it otherwise 404
			var resourceService = context.Nucleus.ResolveSingle<IApplicationResourceService>();
			if (!resourceService.Exists(context, resourcePath))
			{
				// send 404
				context.HttpContext.Response.StatusCode = 404;
				context.HttpContext.Response.StatusDescription = "Not Found";
				return;
			}

			// parse the resource script
			var scriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
			var script = scriptService.Parse(context, resourceService.GetSingle(context, resourcePath));

			// execute the script and write the result back to the output pipe
			outputPipe.Writer.Write(script.Execute<string>(context));
		}
		#endregion
	}
}