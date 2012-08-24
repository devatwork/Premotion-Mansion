using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Dialog
{
	/// <summary>
	/// Redirects the owner page of this dialog to a particular url.
	/// </summary>
	[ScriptTag(Constants.ControlTagNamespaceUri, "invokeDialogParentTrigger")]
	public class InvokeDialogParentTriggerTag : ScriptTag
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationResourceService"></param>
		/// <param name="templateService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public InvokeDialogParentTriggerTag(IApplicationResourceService applicationResourceService, ITemplateService templateService)
		{
			// validate arguments
			if (applicationResourceService == null)
				throw new ArgumentNullException("applicationResourceService");
			if (templateService == null)
				throw new ArgumentNullException("templateService");

			// set values
			this.applicationResourceService = applicationResourceService;
			this.templateService = templateService;
		}
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			// get the arguments
			var arguments = GetAttributes(context);

			// get the action
			string action;
			if (!arguments.TryGetAndRemove(context, "action", out action))
				throw new InvalidOperationException("The action must be specified required.");

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
		#region Private Fields
		private readonly IApplicationResourceService applicationResourceService;
		private readonly ITemplateService templateService;
		#endregion
	}
}