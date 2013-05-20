using System;
using System.Text;
using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.JSon
{
	/// <summary>
	/// Renders a JSON value.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderJsonValue")]
	public class RenderJsonValueTag : ScriptTag
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

			object content;
			if (!TryGetAttribute(context, "value", out content))
			{
				// push a memory pipe to the stack
				var buffer = new StringBuilder();
				using (var pipe = new StringOutputPipe(buffer))
				using (context.OutputPipeStack.Push(pipe))
					ExecuteChildTags(context);
				content = buffer.ToString();
			}

			// write the attribute
			outputPipe.JsonWriter.WriteValue(content);
		}
	}
}