using System;

namespace Premotion.Mansion.Web.Controls.Forms.Actions
{
	///<summary>
	/// Implemens the Prepare delegate action.
	///</summary>
	public class PrepareDelegateAction : Action
	{
		#region Constructors
		/// <summary>
		/// Constructs a prepare delegate action.
		/// </summary>
		/// <param name="action">The action which to execute when preparing.</param>
		public PrepareDelegateAction(Action<IMansionWebContext, Form, Step> action)
		{
			// validate arguments
			if (action == null)
				throw new ArgumentNullException("action");

			// set values
			this.action = action;
		}
		#endregion
		#region Overrides of Action
		/// <summary>
		/// Prepares the <paramref name="step"/> before the <paramref name="form"/> is rendered.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/>.</param>
		/// <param name="step">The <see cref="Step"/>.</param>
		protected override void Prepare(IMansionWebContext context, Form form, Step step)
		{
			action(context, form, step);
		}
		#endregion
		#region Private Fields
		private readonly Action<IMansionWebContext, Form, Step> action;
		#endregion
	}
}