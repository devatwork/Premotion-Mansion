using System;
using System.Text;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Core.ScriptFunctions.Rendering
{
	/// <summary>
	/// Renders a section and returns it's content.
	/// </summary>
	[ScriptFunction("RenderSection")]
	public class RenderSection : FunctionExpression
	{
		/// <summary>
		/// Renders a section and returns it's content.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render..</param>
		/// <returns></returns>
		public string Evaluate(MansionContext context, string sectionName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException("sectionName");

			// render the control
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
				context.Nucleus.Get<ITemplateService>(context).Render(context, sectionName, TemplateServiceConstants.OutputTargetField).Dispose();

			// return the buffer
			return buffer.ToString();
		}
	}
}