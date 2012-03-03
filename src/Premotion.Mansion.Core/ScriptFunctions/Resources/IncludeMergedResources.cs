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

			// get the services
			var resourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var scriptService = context.Nucleus.Get<IExpressionScriptService>(context);

			// get the resource
			var resourcePath = resourceService.ParsePath(context, new PropertyBag
			                                                      {
			                                                      	{"path", relativePath}
			                                                      });

			// create the buffer
			var buffer = new StringBuilder();

			// loop over all the resources
			foreach (var script in resourceService.Get(context, resourcePath).Select(resource => scriptService.Parse(context, resource)))
			{
				// execute the script and write the result back to the output pipe
				buffer.AppendLine(script.Execute<string>(context));
			}

			return buffer.ToString();
		}
	}
}