using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
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
				// start the document
				jsonOutputPipe.JsonWriter.WriteStartArray();

				// execute the children
				ExecuteChildTags(context);

				// finish the document
				jsonOutputPipe.JsonWriter.WriteEndArray();
			}
		}
	}
}