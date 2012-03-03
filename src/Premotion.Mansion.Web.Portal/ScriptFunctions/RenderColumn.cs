using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;
using Premotion.Mansion.Core.Templating;
using Premotion.Mansion.Web.Portal.Service;

namespace Premotion.Mansion.Web.Portal.ScriptFunctions
{
	/// <summary>
	/// Renders the specified block column.
	/// </summary>
	[ScriptFunction("renderColumn")]
	public class RenderColumn : FunctionExpression
	{
		/// <summary>
		/// Renders a column with the specified <paramref name="columnName"/> to the output pipe.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="columnName">The name of the column which to render.</param>
		/// <param name="ownerProperties">The <see cref="IPropertyBag"/> to which the column belongs.</param>
		/// <param name="blockDataset">The <see cref="Dataset"/> containing the all blocks of the <paramref name="ownerProperties"/>.</param>
		/// <returns>Returns the HTML for this column.</returns>
		public string Evaluate(MansionContext context, string columnName, IPropertyBag ownerProperties, Dataset blockDataset)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(columnName))
				throw new ArgumentNullException("columnName");
			if (ownerProperties == null)
				throw new ArgumentNullException("ownerProperties");
			if (blockDataset == null)
				throw new ArgumentNullException("blockDataset");

			// get the services
			var portalService = context.Nucleus.Get<IPortalService>(context);

			// render the column
			var buffer = new StringBuilder();
			using (var pipe = new StringOutputPipe(buffer))
			using (context.OutputPipeStack.Push(pipe))
				portalService.RenderColumn(context, columnName, ownerProperties, blockDataset, TemplateServiceConstants.OutputTargetField);

			// return the bufferred content
			return buffer.ToString();
		}
	}
}