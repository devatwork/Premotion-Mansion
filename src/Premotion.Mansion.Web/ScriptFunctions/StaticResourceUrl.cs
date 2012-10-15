using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Web.Hosting;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Generates a path for static resources.
	/// </summary>
	[ScriptFunction("StaticResourceUrl")]
	public class StaticResourceUrl : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public StaticResourceUrl(IApplicationResourceService applicationResourceService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");

			// set values
			this.applicationResourceService = applicationResourceService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Generates a path for static resources.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <param name="relativePath">The relative path to the resource.</param>
		/// <returns>The <see cref="Uri"/> ot the static resource.</returns>
		public Url Evaluate(IMansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			// get the web context
			var webContext = context.Cast<IMansionWebContext>();

			// create the url
			var url = Url.CreateUrl(webContext);

			// create the relative path
			url.PathSegments = WebUtilities.CombineIntoRelativeUrl(StaticResourceRequestHandler.Prefix, relativePath);

			// create the uri
			return url;
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		#endregion
	}
}