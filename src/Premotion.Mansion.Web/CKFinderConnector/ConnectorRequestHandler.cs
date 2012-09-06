using System;
using System.Collections.Generic;
using Premotion.Mansion.Web.CKFinderConnector.Handlers;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// Implements the <see cref="MansionRequestHandlerBase"/> for CKFinder connector requests.
	/// </summary>
	public class ConnectorRequestHandler : MansionRequestHandlerBase
	{
		#region Constants
		private const string Prefix = "CKFinder.Connector";
		#endregion
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public ConnectorRequestHandler() : base(15, new UrlPrefixSpeficiation(Prefix))
		{
		}
		#endregion
		#region Overrides of MansionRequestHandlerBase
		/// <summary>e
		/// 
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		protected override void DoExecute(IMansionWebContext context)
		{
			// get the command name
			var commandName = context.HttpContext.Request.QueryString["command"];
			if (string.IsNullOrEmpty(commandName))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// get the command handler for the command
			CommandHandlerBase handler;
			if (!Handlers.TryGetValue(commandName, out handler))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// handle the command
			handler.Handle(context);
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
		private static readonly IDictionary<string, CommandHandlerBase> Handlers = new Dictionary<string, CommandHandlerBase>(StringComparer.OrdinalIgnoreCase)
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