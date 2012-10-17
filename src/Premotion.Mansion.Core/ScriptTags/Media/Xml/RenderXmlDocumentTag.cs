using Premotion.Mansion.Core.IO.Xml;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Xml
{
	/// <summary>
	/// Renders an XML document to the top most outpipe.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderXmlDocument")]
	public class RenderXmlDocumentTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// create the XML pipe and push it to the stack
			using (var xmlOutputPipe = new XmlOutputPipe(context.OutputPipe))
			using (context.OutputPipeStack.Push(xmlOutputPipe))
			{
				// add the default namespace
				string defaultNamespace;
				if (TryGetAttribute(context, "defaultNamespace", out defaultNamespace))
					xmlOutputPipe.NamespaceManager.AddNamespace(string.Empty, defaultNamespace);

				// start the document
				xmlOutputPipe.XmlWriter.WriteStartDocument();

				// execute the children
				ExecuteChildTags(context);

				// finish the document
				xmlOutputPipe.XmlWriter.WriteEndDocument();
			}
		}
	}
}