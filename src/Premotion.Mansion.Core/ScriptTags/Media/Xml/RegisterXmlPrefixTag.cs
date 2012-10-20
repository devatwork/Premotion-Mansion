using System;
using Premotion.Mansion.Core.IO.Xml;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Media.Xml
{
	/// <summary>
	/// Registers a namespace prefix in an XML document.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "registerXmlPrefix")]
	public class RegisterXmlPrefixTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the variables
			var prefix = GetRequiredAttribute<string>(context, "prefix");
			var namespaceUri = GetRequiredAttribute<string>(context, "namespace");

			// get the XML output pipe
			var outputPipe = context.OutputPipe as XmlOutputPipe;
			if (outputPipe == null)
				throw new InvalidOperationException("No XML output pipe found on thet stack. Open an XML output pipe first.");

			// check if the prefix has already been registered
			if (outputPipe.NamespaceManager.HasNamespace(prefix))
			{
				var otherNamespace = outputPipe.NamespaceManager.LookupNamespace(prefix);
				if (!namespaceUri.Equals(otherNamespace, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException(string.Format("Could not register namespace '{0}' with prefix '{1}'. The prefix is already registered for namespace '{2}'.", namespaceUri, prefix, otherNamespace));
			}
			else
			{
				// register the prefix
				outputPipe.NamespaceManager.AddNamespace(prefix, namespaceUri);
			}
		}
	}
}