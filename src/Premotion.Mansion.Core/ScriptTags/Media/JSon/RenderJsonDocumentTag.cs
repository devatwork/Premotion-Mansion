﻿using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.JSon
{
	/// <summary>
	/// Renders a JSON document to the top most outpipe.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderJsonDocument")]
	public class RenderJsonDocumentTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// create the XML pipe and push it to the stack
			using (var jsonOutputPipe = new JsonOutputPipe(context.OutputPipe))
			using (context.OutputPipeStack.Push(jsonOutputPipe))
			{
				// execute the children
				ExecuteChildTags(context);
			}
		}
	}
}