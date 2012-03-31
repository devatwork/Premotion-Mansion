using System;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.ScriptFunctions.Resources
{
	/// <summary>
	/// Includes a dynamic resource at the current location.
	/// </summary>
	[ScriptFunction("IncludeMergedResources")]
	public class IncludeMergedResources : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="expressionScriptService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public IncludeMergedResources(IApplicationResourceService applicationResourceService, IExpressionScriptService expressionScriptService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (expressionScriptService == null)
				throw new ArgumentNullException("expressionScriptService");

			// set values
			this.applicationResourceService = applicationResourceService;
			this.expressionScriptService = expressionScriptService;
		}
		#endregion
		#region Evalaluate Methods
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

			// create the buffer
			var buffer = new StringBuilder();

			// loop over all the resources
			foreach (var script in applicationResourceService.Get(context, resourcePath).Select(resource => expressionScriptService.Parse(context, resource)))
			{
				// execute the script and write the result back to the output pipe
				buffer.AppendLine(script.Execute<string>(context));
			}

			return buffer.ToString();
		}
		#endregion
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly IExpressionScriptService expressionScriptService;
		#endregion
	}
}