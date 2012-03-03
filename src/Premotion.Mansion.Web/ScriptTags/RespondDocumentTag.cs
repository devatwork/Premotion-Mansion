using System;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Responds a page to the browser.
	/// </summary>
	[Named(Constants.NamespaceUri, "respondDocument")]
	public class RespondDocumentTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the web request
			var webRequest = context.Cast<MansionWebContext>();

			var outputPipe = (WebOutputPipe) context.OutputPipeStack.FirstOrDefault(x => x is WebOutputPipe);
			if (outputPipe == null)
				throw new InvalidOperationException("No web output pipe was found on thet stack.");

			// set the properties of the output pipe
			outputPipe.ContentType = GetAttribute(webRequest, "contentType", "text/html");
			outputPipe.Encoding = GetAttribute(webRequest, "encoding", Encoding.UTF8);
			outputPipe.OutputCacheEnabled = GetAttribute(webRequest, "cache", true);

			// execute the children
			ExecuteChildTags(webRequest);
		}
		#endregion
	}
}