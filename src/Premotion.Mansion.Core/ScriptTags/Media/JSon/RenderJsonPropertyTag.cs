using System;
using System.Text;
using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.JSon
{
	/// <summary>
	/// Renders a JSON property.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderJsonProperty")]
	public class RenderJsonPropertyTag : ScriptTag
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
			var propertyName = GetAttribute<string>(context, "propertyName");
			if (string.IsNullOrEmpty(propertyName))
				throw new InvalidOperationException("The propertyName attribute is required.");

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
			outputPipe.JsonWriter.WritePropertyName(propertyName);
			outputPipe.JsonWriter.WriteValue(content);
		}
	}
}