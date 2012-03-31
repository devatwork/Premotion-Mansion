using System;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Resources
{
	/// <summary>
	/// Includes a dynamic resource at the current location.
	/// </summary>
	[ScriptFunction("IncludeDynamicResource")]
	public class IncludeDynamicResource : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IncludeDynamicResource(IApplicationResourceService applicationResourceService)
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
		/// </summary>
		/// <param name="context"></param>
		/// <param name="relativePath">The relative path to the resource which to include.</param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			// get the resource
			var resourcePath = applicationResourceService.ParsePath(context, new PropertyBag
			                                                                 {
			                                                                 	{"path", relativePath}
			                                                                 });

			// parse the resource script
			var scriptService = context.Nucleus.ResolveSingle<IExpressionScriptService>();
			var script = scriptService.Parse(context, applicationResourceService.GetSingle(context, resourcePath));

			// execute the script and write the result back to the output pipe
			return script.Execute<string>(context);
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		#endregion
	}
}