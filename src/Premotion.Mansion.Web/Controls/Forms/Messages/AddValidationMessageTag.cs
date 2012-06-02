using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Messages
{
	/// <summary>
	/// Adds a validation message to the current form.
	/// </summary>
	[ScriptTag(Constants.FormTagNamespaceUri, "addValidationMessage")]
	public class AddValidationMessageTag : ScriptTag
	{
		#region Constructors
		#endregion
		#region Overrides of ScriptTag
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoExecute(IMansionContext context)
		{
			var webContext = context.Cast<IMansionWebContext>();
			var form = webContext.CurrentForm;

			// get the attributes
			var message = GetRequiredAttribute<string>(context, "message");
			var controlName = GetRequiredAttribute<string>(context, "controlName");

			// find the control
			Control control;
			if (!form.TryFindControlByName(context, controlName, out control))
				throw new InvalidOperationException(string.Format("Could not find control with id '{0}'", controlName));

			// add the validation message
			form.ValidationResults.AddResult(context, message, control);
		}
		#endregion
	}
}