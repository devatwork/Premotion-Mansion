using System;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Renders a JSON object.
	/// </summary>
	[Named(Constants.NamespaceUri, "renderJsonObject")]
	public class RenderJsonObjectTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);

			// get the XML output pipe
			var outputPipe = context.OutputPipe as JsonOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No JSON output pipe found on thet stack. Open an JSON output pipe first.");

			// start the element
			outputPipe.JsonWriter.WriteStartObject();

			// render the attributes
			foreach (var attribute in attributes)
			{
				outputPipe.JsonWriter.WritePropertyName(attribute.Key);
				outputPipe.JsonWriter.WriteValue(attribute.Value);
			}

			// render the children
			ExecuteChildTags(context);

			// finish the element
			outputPipe.JsonWriter.WriteEndObject();
		}
	}
}