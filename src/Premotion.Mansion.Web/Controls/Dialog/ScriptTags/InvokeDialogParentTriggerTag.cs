using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Dialog.ScriptTags
{
	/// <summary>
	/// Redirects the owner page of this dialog to a particular url.
	/// </summary>
	[Named(Constants.ControlTagNamespaceUri, "invokeDialogParentTrigger")]
	public class InvokeDialogParentTriggerTag : ScriptTag
	{
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected override void DoExecute(MansionContext context)
		{
			// get the arguments
			var arguments = GetAttributes(context);

			// get the action
			string action;
			if (!arguments.TryGetAndRemove(context, "action", out action))
				throw new InvalidOperationException("The action must be specified required.");

			// get the services
			var applicationResourceService = context.Nucleus.Get<IApplicationResourceService>(context);
			var templateService = context.Nucleus.Get<ITemplateService>(context);

			// open the control template
			var controlTemplateResourcePath = applicationResourceService.ParsePath(context, Control.ControlTemplatePathProperties);
			var controlTemplateResource = applicationResourceService.Get(context, controlTemplateResourcePath);
			using (templateService.Open(context, controlTemplateResource))
			using (context.Stack.Push("TriggerProperties", new PropertyBag
			                                               {
			                                               	{"action", action}
			                                               }))
			using (context.Stack.Push("TriggerArguments", arguments))
				templateService.Render(context, "InvokeDialogParentTrigger" + (arguments.Count > 0 ? "WithParameters" : string.Empty)).Dispose();
		}
		#endregion
	}
}