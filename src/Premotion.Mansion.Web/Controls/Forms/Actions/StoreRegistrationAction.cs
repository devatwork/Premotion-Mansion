using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Actions
{
	/// <summary>
	/// Implements an processing <see cref="Action"/> using <see cref="IScript"/>.
	/// </summary>
	public class StoreRegistrationAction : Action
	{
		#region Nested type: StoreRegistrationActionFactoryTag
		/// <summary>
		/// Constructs <see cref="StoreRegistrationAction"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "storeRegistrationAction")]
		public class StoreRegistrationActionFactoryTag : ActionFactoryTag<StoreRegistrationAction>
		{
			#region Overrides of ActionFactoryTag<StoreRegistrationAction>
			/// <summary>
			/// Creates an instance of the <see cref="Action"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="properties">The properties of the action.</param>
			/// <returns>Returns the created <see cref="Action"/>.</returns>
			protected override StoreRegistrationAction Create(IMansionWebContext context, IPropertyBag properties)
			{
				// get the values
				var supportedActions = GetAttribute(context, "supportedActions", string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
				var requiresValidForm = GetAttribute(context, "requiresValidForm", true);

				// retrieve the index node
				var registrationIndexNode = properties.Get<Node>(context, "parentSource");

				// create and return the action
				return new StoreRegistrationAction(supportedActions, requiresValidForm, registrationIndexNode);
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
		/// <param name="registrationIndexNode">The index node of the registrations.</param>
		private StoreRegistrationAction(string[] supportedActions, bool requiresValidForm, Node registrationIndexNode)
		{
			// validate arguments
			if (supportedActions == null)
				throw new ArgumentNullException("supportedActions");
			if (registrationIndexNode == null)
				throw new ArgumentNullException("registrationIndexNode");

			// set values
			this.supportedActions = supportedActions;
			this.requiresValidForm = requiresValidForm;
			this.registrationIndexNode = registrationIndexNode;
		}
		#endregion
		#region Overrides of Action
		/// <summary>
		/// Processes the <paramref name="step"/> after the <paramref name="form"/> is submitted back to the server with valid data.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected override void Process(IMansionWebContext context, Form form, Step step)
		{
			// check if this action requires a valid form
			if (requiresValidForm && !form.ValidationResults.IsValid)
				return;

			// check if the action should be executed
			if (supportedActions.Length > 0 && !supportedActions.Contains(form.State.CurrentAction, StringComparer.OrdinalIgnoreCase))
				return;

			// assemble the registration properties
			var properties = new PropertyBag();
			properties.Merge(form.State.FieldProperties);
			properties.Set("type", "Registration");
			if (!properties.Contains("name"))
				properties.Set("name", DateTime.Now.ToString(context.UserInterfaceCulture));

			// store the registration
			context.Repository.CreateNode(context, registrationIndexNode, properties);
		}
		#endregion
		#region Private Fields
		private readonly Node registrationIndexNode;
		private readonly bool requiresValidForm;
		private readonly string[] supportedActions;
		#endregion
	}
}