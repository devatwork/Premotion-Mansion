using System;
using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.JSon
{
	/// <summary>
	/// Renders a JSON array.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderJsonArray")]
	public class RenderJsonArrayTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the XML output pipe
			var outputPipe = context.OutputPipe as JsonOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No JSON output pipe found on thet stack. Open an JSON output pipe first.");

			// start the element
			outputPipe.JsonWriter.WriteStartArray();

			// render the children
			ExecuteChildTags(context);

			// finish the element
			outputPipe.JsonWriter.WriteEndArray();
		}
	}
}