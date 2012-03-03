using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders the specified block.
	/// </summary>
	[ScriptFunction("renderBlock")]
	public class RenderBlock : FunctionExpression
	{
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <returns>Returns the HTML for this block.</returns>
		public string Evaluate(MansionContext context, IPropertyBag blockProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (blockProperties == null)
				throw new ArgumentNullException("blockProperties");

			// get the services
			var portalService = context.Nucleus.Get<IPortalService>(context);

			// render the block
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
				portalService.RenderBlock(context, blockProperties, TemplateServiceConstants.OutputTargetField);

			// return the bufferred content
			return buffer.ToString();
		}
	}
}