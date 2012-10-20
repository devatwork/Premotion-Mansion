using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.ScriptTags
{
	/// <summary>
	/// Responds a page to the browser.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "respondDocument")]
	public class RespondDocumentTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the output pipe
			var outputPipe = context.GetWebOuputPipe();

			// set the properties of the output pipe
			outputPipe.Response.ContentType = GetAttribute(context, "contentType", "text/html");
			outputPipe.Encoding = GetAttribute(context, "encoding", Encoding.UTF8);
			outputPipe.Response.CacheSettings.OutputCacheEnabled = GetAttribute(context, "cache", true);

			// execute the children
			ExecuteChildTags(context);
		}
		#endregion
	}
}