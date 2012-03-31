using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Represents an executable action.
	/// </summary>
	public abstract class Action
	{
		#region Nested type: ActionFactoryTag
		///<summary>
		/// Constructs <see cref="Action"/>.
		///</summary>
		public abstract class ActionFactoryTag<TAction> : ScriptTag where TAction : Action
		{
			#region Overrides of ScriptTag
			/// <summary>
			/// Executes this tag.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			protected override void DoExecute(IMansionContext context)
			{
				// get the mansion web context
				var webContext = context.Cast<IMansionWebContext>();

				// get the properties of this control
				var properties = GetAttributes(context);

				// create the action
				var action = Create(webContext, properties);

				// add id to the control
				FormActionContainer actionContainer;
				if (!webContext.TryPeekControl(out actionContainer))
					throw new InvalidOperationException(string.Format("Action '{0}' can only be applied to types inheriting from '{1}'", GetType(), typeof (FormActionContainer)));

				// add the rule
				actionContainer.Add(action);
			}
			/// <summary>
			/// Creates an instance of the <see cref="Action"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="properties">The properties of the action.</param>
			/// <returns>Returns the created <see cref="Action"/>.</returns>
			protected abstract TAction Create(IMansionWebContext context, IPropertyBag properties);
			#endregion
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this action.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		public void Execute(IMansionWebContext context, Form form, Step step)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (step == null)
				throw new ArgumentNullException("step");

			// check if the pre or post action should be execute
			if (form.State.IsPostback)
				Process(context, form, step);
			else
				Prepare(context, form, step);
		}
		/// <summary>
		/// Prepares the <paramref name="step"/> before the <paramref name="form"/> is rendered.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected virtual void Prepare(IMansionWebContext context, Form form, Step step)
		{
			// do nothing
		}
		/// <summary>
		/// Processes the <paramref name="step"/> after the <paramref name="form"/> is submitted back to the server with valid data.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected virtual void Process(IMansionWebContext context, Form form, Step step)
		{
			// do nothing
		}
		#endregion
	}
}