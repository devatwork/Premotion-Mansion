using System;
using Premotion.Mansion.Core.IO.Xml;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Xml
{
	/// <summary>
	/// Renders an XML element to the XML output pipe. This tag must be used within and <see cref="RenderXmlDocumentTag"/>.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "renderXmlElement")]
	public class RenderXmlElementTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the attributes
			var attributes = GetAttributes(context);
			string elementName;
			if (! attributes.TryGetAndRemove(context, "elementName", out elementName) || string.IsNullOrEmpty(elementName))
				throw new InvalidOperationException("The elementName attribute can not be empty");

			// get the XML output pipe
			var outputPipe = context.OutputPipe as XmlOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No XML output pipe found on thet stack. Open an XML output pipe first.");

			// get the namespace uri
			string elementPrefix;
			string elementNamespace = null;
			if ((attributes.TryGetAndRemove(context, "elementPrefix", out elementPrefix) && !string.IsNullOrEmpty(elementPrefix)) || !string.IsNullOrEmpty(outputPipe.NamespaceManager.DefaultNamespace))
			{
				// get the namespace of this prefix
				elementNamespace = outputPipe.NamespaceManager.LookupNamespace(elementPrefix ?? string.Empty);
				if (string.IsNullOrEmpty(elementNamespace))
					throw new InvalidOperationException(elementPrefix + " is not a registered prefix");
			}

			// start the element
			outputPipe.XmlWriter.WriteStartElement(elementPrefix, elementName, elementNamespace);

			// render the attributes
			foreach (var attributeName in attributes.Names)
			{
				outputPipe.XmlWriter.WriteStartAttribute(attributeName);
				outputPipe.XmlWriter.WriteString(GetAttribute<string>(context, attributeName));
				outputPipe.XmlWriter.WriteEndAttribute();
			}

			// render the children
			ExecuteChildTags(context);

			// finish the element
			outputPipe.XmlWriter.WriteEndElement();
		}
	}
}