using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Scripting;
using ActionDelegate = System.Action;

namespace Premotion.Mansion.Web.Controls.Forms.Actions
{
	/// <summary>
	/// Implements an processing <see cref="Action"/> using <see cref="IScript"/>.
	/// </summary>
	public class ProcessScriptAction : Action
	{
		#region Nested type: ProcessScriptActionFactoryTag
		/// <summary>
		/// Constructs <see cref="ProcessScriptAction"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "processScriptAction")]
		public class ProcessScriptActionFactoryTag : ActionFactoryTag<ProcessScriptAction>
		{
			#region Overrides of ActionFactoryTag<ProcessScriptAction>
			/// <summary>
			/// Creates an instance of the <see cref="Action"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="properties">The properties of the action.</param>
			/// <returns>Returns the created <see cref="Action"/>.</returns>
			protected override ProcessScriptAction Create(MansionWebContext context, IPropertyBag properties)
			{
				// get the values
				var supportedActions = GetAttribute(context, "supportedActions", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				var requiresValidForm = GetAttribute(context, "requiresValidForm", true);

				// create and return the action
				return new ProcessScriptAction(supportedActions, requiresValidForm, () => ExecuteChildTags(context), properties);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs.
		/// </summary>
		/// <param name="supportedActions">The action on which this action should be executed.</param>
		/// <param name="requiresValidForm">Flag indicating whether the form should be valid before executing this action.</param>
		/// <param name="action">The <see cref="IScript"/> which to execute.</param>
		/// <param name="properties">The properties of this action.</param>
		private ProcessScriptAction(string[] supportedActions, bool requiresValidForm, ActionDelegate action, IPropertyBag properties)
		{
			// validate arguments
			if (supportedActions == null)
				throw new ArgumentNullException("supportedActions");
			if (action == null)
				throw new ArgumentNullException("action");
			if (properties == null)
				throw new ArgumentNullException("properties");

			// set values
			this.supportedActions = supportedActions;
			this.properties = properties;
			this.action = action;
			this.requiresValidForm = requiresValidForm;
		}
		#endregion
		#region Overrides of Action
		/// <summary>
		/// Processes the <paramref name="step"/> after the <paramref name="form"/> is submitted back to the server with valid data.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected override void Process(MansionWebContext context, Form form, Step step)
		{
			// check if this action requires a valid form
			if (requiresValidForm && !form.ValidationResults.IsValid)
				return;

			// check if the action should be executed
			if (supportedActions.Length > 0 && !supportedActions.Contains(form.State.CurrentAction, StringComparer.OrdinalIgnoreCase))
				return;

			// execute the action
			using (context.Stack.Push("ActionProperties", properties, false))
				action();
		}
		#endregion
		#region Private Fields
		private readonly ActionDelegate action;
		private readonly IPropertyBag properties;
		private readonly bool requiresValidForm;
		private readonly string[] supportedActions;
		#endregion
	}
}