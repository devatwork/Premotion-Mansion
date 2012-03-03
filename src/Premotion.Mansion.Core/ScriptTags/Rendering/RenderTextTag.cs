using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Core.ScriptTags.Rendering
{
	/// <summary>
	/// Renders a text to the current output pipe
	/// </summary>
	[Named(Constants.NamespaceUri, "renderText")]
	public class RenderTextTag : ScriptTag
	{
		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		protected override void DoExecute(MansionContext context)
		{
			// get the message
			var message = GetAttribute<string>(context, "message") ?? GetContent<string>(context);

			// write the message to the output pip
			context.OutputPipe.Writer.Write(message);
		}
	}
}