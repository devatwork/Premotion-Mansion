using System.Linq;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Disables the response template cache.
	/// </summary>
	[ScriptTag(Constants.NamespaceUri, "disableResponseTemplateCache")]
	public class DisableResponseTemplateCacheTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The application context.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// find all the response template caches and disable their caches
			foreach (var responsePipe in context.OutputPipeStack.OfType<ResponseTemplateTag.ResponseOutputPipe>())
				responsePipe.ResponseCacheEnabled = false;
		}
		#endregion
	}
}