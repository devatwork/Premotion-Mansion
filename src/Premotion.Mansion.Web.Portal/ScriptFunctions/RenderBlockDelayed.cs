using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Portal.Descriptors;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders the specified delayed block.
	/// </summary>
	[ScriptFunction("RenderBlockDelayed")]
	public class RenderBlockDelayed : FunctionExpression
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

			// first get the block behavior
			var blockType = blockProperties.Get<ITypeDefinition>(context, "type");
			DelayedRenderingBlockBehaviorDescriptor behavior;
			if (!blockType.TryFindDescriptorInHierarchy(out behavior))
				throw new InvalidOperationException(string.Format("Block type '{0}' does not have a behavior", blockType.Name));

			// render the block
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
				behavior.RenderDelayed(context, blockProperties, TemplateServiceConstants.OutputTargetField);

			// return the bufferred content
			return buffer.ToString();
		}
	}
}