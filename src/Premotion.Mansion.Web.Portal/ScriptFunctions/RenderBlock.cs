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
	[ScriptFunction("RenderBlock")]
	public class RenderBlock : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="portalService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RenderBlock(IPortalService portalService)
		{
			// validate arguments
			if (portalService == null)
				throw new ArgumentNullException("portalService");

			// set values
			this.portalService = portalService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Renders the specified <paramref name="blockProperties"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="blockProperties">The <see cref="IPropertyBag"/> of the block which to render.</param>
		/// <returns>Returns the HTML for this block.</returns>
		public string Evaluate(IMansionContext context, IPropertyBag blockProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (blockProperties == null)
				throw new ArgumentNullException("blockProperties");

			// render the block
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
				portalService.RenderBlock(context, blockProperties, TemplateServiceConstants.OutputTargetField);

			// return the bufferred content
			return buffer.ToString();
		}
		#endregion
		#region Private Fields
		private readonly IPortalService portalService;
		#endregion
	}
}