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
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderSection(ITemplateService templateService)
		{
			// validaet arguments
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.templateService = templateService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Renders a section and returns it's content.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render..</param>
		/// <returns></returns>
		public string Evaluate(IMansionContext context, string sectionName)
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
				templateService.Render(context, sectionName, TemplateServiceConstants.OutputTargetField).Dispose();

			// return the buffer
			return buffer.ToString();
		}
		#endregion
		#region Private Fields
		private readonly ITemplateService templateService;
		#endregion
	}
}