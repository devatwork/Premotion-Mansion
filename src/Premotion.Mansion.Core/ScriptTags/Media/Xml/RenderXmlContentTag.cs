using System;
using Premotion.Mansion.Core.IO.Xml;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Xml
{
	/// <summary>
	/// Renders the content of this tag to the XML output pipe. This tag must be used within and <see cref="RenderXmlDocumentTag"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderXmlContent")]
	public class RenderXmlContentTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the XML output pipe
			var outputPipe = context.OutputPipe as XmlOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No XML output pipe found on thet stack. Open an XML output pipe first.");

			// get the content
			var content = GetContent<string>(context);

			//  check whether the content should be wrapped in a CDATA section or not
			if (GetAttribute(context, "inCData", false))
				outputPipe.XmlWriter.WriteCData(content);
			else
				outputPipe.XmlWriter.WriteValue(content);
		}
	}
}