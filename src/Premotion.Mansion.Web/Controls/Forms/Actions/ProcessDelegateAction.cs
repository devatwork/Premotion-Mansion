using System;
using System.Collections.Generic;
using System.Linq;

namespace Premotion.Mansion.Web.Controls.Forms.Actions
{
	///<summary>
	/// Implemens the Process delegate action.
	///</summary>
	public class ProcessDelegateAction : Action
	{
		#region Constructors
		/// <summary>
		/// Constructs a process delegate action.
		/// </summary>
		/// <param name="action">The action which to execute when preparing.</param>
		public ProcessDelegateAction(Action<IMansionWebContext, Form, Step> action) : this(new string[0], action)
		{
		}
		/// <summary>
		/// Constructs a process delegate action.
		/// </summary>
		/// <param name="actions">The actions on which to execute.</param>
		/// <param name="action">The action which to execute when preparing.</param>
		public ProcessDelegateAction(IEnumerable<string> actions, Action<IMansionWebContext, Form, Step> action)
		{
			// validate arguments
			if (actions == null)
				throw new ArgumentNullException("actions");
			if (action == null)
				throw new ArgumentNullException("action");

			// set values
			this.actions = actions.ToList();
			this.action = action;
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
			// filter on actions
			if (actions.Count != 0 && !actions.Contains(form.State.CurrentAction))
				return;

			action(context, form, step);
		}
		#endregion
		#region Private Fields
		private readonly Action<IMansionWebContext, Form, Step> action;
		private readonly List<string> actions;
		#endregion
	}
}