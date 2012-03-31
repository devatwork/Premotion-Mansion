using System;
using Premotion.Mansion.Core.IO.JSon;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media
{
	/// <summary>
	/// Renders a JSON object.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderJsonArrayProperty")]
	public class RenderJsonArrayPropertyTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);
			string propertyName;
			if (!attributes.TryGetAndRemove(context, "propertyName", out propertyName) || string.IsNullOrEmpty(propertyName))
				throw new InvalidOperationException("The propertyName attribute can not be empty.");

			// get the XML output pipe
			var outputPipe = context.OutputPipe as JsonOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No JSON output pipe found on thet stack. Open an JSON output pipe first.");

			// start the element
			outputPipe.JsonWriter.WritePropertyName(propertyName);
			outputPipe.JsonWriter.WriteStartArray();

			// render the attributes
			foreach (var attribute in attributes)
				outputPipe.JsonWriter.WriteValue(attribute.Value);

			// render the children
			ExecuteChildTags(context);

			// finish the element
			outputPipe.JsonWriter.WriteEndArray();
		}
	}
}