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
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="relativePath">The relative path to the resource which to include.</param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string relativePath)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			// get the resource
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var resourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                      {
			                                                      	{"path", relativePath}
			                                                      });

			// parse the resource script
			var scriptService = context.Nucleus.Get<IExpressionScriptService>(context);
			var script = scriptService.Parse(context, resourceService.GetSingle(context, resourcePath));

			// execute the script and write the result back to the output pipe
			return script.Execute<string>(context);
		}
	}
}