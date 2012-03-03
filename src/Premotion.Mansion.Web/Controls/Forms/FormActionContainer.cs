using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// A special type of <see cref="FormControlContainer"/> which contains <see cref="Action"/>s.
	/// </summary>
	public abstract class FormActionContainer : FormControlContainer
	{
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected FormActionContainer(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Add Methods
		/// <summary>
		/// Adds an <paramref name="action"/> to this action container.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> which to add.</param>
		public void Add(Action action)
		{
			// validate arguments
			if (action == null)
				throw new ArgumentNullException("action");
			actions.Add(action);
		}
		#endregion
		#region Lifecycle Methods
		/// <summary>
		/// Execute the <see cref="Action"/>s on this object..
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected static void ExecuteActions(MansionWebContext context, Form form, Step step)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");

			// first execute actions on step
			step.DoExecuteActions(context, form, step);

			// finally execute actions on form
			form.DoExecuteActions(context, form, step);
		}
		/// <summary>
		/// Execute the <see cref="Action"/>s on this object..
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		private void DoExecuteActions(MansionWebContext context, Form form, Step step)
		{
			// execute all actions
			foreach (var action in actions)
				action.Execute(context, form, step);
		}
		#endregion
		#region Private Fields
		private readonly List<Action> actions = new List<Action>();
		#endregion
	}
}