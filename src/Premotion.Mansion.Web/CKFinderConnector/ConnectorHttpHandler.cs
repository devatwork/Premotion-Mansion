using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using Premotion.Mansion.Web.CKFinderConnector.Handlers;
using Premotion.Mansion.Web.Http;

namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// Implements the <see cref="IHttpHandler"/> for CKFinder connector requests.
	/// </summary>
	public class ConnectorHttpHandler : MansionHttpHandlerBase, IRequiresSessionState
	{
		#region Overrides of MansionHttpHandlerBase
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/> constructed for handling the current request.</param>
		protected override void ProcessRequest(MansionWebContext context)
		{
			// get the command name
			var commandName = context.HttpContext.Request.QueryString["command"];
			if (string.IsNullOrEmpty(commandName))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// get the command handler for the command
			CommandHandlerBase handler;
			if (!handlers.TryGetValue(commandName, out handler))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// handle the command
			handler.Handle(context);
		}
		#endregion
		#region Private Fields
		private static readonly IDictionary<string, CommandHandlerBase> handlers = new Dictionary<string, CommandHandlerBase>(StringComparer.OrdinalIgnoreCase)
		                                                                           {
		                                                                           	{"Init", new InitCommandHandler()},
		                                                                           	{"GetFolders", new GetFoldersCommandHandler()},
		                                                                           	{"GetFiles", new GetFilesCommandHandler()},
		                                                                           	{"LoadCookies", new LoadCookiesCommandHandler()},
		                                                                           	{"FileUpload", new FileUploadCommandHandler()},
		                                                                           	{"CreateFolder", new CreateFolderCommandHandler()},
		                                                                           };
		#endregion
	}
}