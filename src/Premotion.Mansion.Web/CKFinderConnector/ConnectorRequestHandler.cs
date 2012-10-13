using System;
using System.Collections.Generic;
using Premotion.Mansion.Core;
using Premotion.Mansion.Web.CKFinderConnector.Handlers;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.CKFinderConnector
{
	/// <summary>
	/// Implements the <see cref="RequestHandler"/> for CKFinder connector requests.
	/// </summary>
	public class ConnectorRequestHandler : RequestHandler
	{
		#region Constants
		private const string Prefix = "CKFinder.Connector";
		#endregion
		#region Nested type: ConnectorRequestHandlerFactory
		/// <summary>
		/// </summary>
		public class ConnectorRequestHandlerFactory : SpecificationRequestHandlerFactory
		{
			#region Constructors
			/// <summary></summary>
			public ConnectorRequestHandlerFactory() : base(new UrlPrefixSpecification(Prefix))
			{
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
				return new ConnectorRequestHandler();
			}
			#endregion
		}
		#endregion
		#region Overrides of MansionRequestHandlerBase
		/// <summary>
		/// Executes the handler within the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/> in which to execute the current request.</param>
		/// <returns>Returns the <see cref="WebResponse"/>.</returns>
		protected override WebResponse DoExecute(IMansionWebContext context)
		{
			// create the response
			var response = WebResponse.Create(context);

			// get the command name
			var commandName = context.Request.QueryString["command"];
			if (string.IsNullOrEmpty(commandName))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// get the command handler for the command
			CommandHandlerBase handler;
			if (!Handlers.TryGetValue(commandName, out handler))
				throw new ConnectorException(ErrorCodes.InvalidCommand);

			// handle the command
			handler.Handle(context, response);

			// return the response
			return response;
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