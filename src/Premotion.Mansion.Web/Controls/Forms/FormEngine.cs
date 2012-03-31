using System;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// <see cref="Form"/> engines drive forms.
	/// </summary>
	public abstract class FormEngine
	{
		#region State Methods
		/// <summary>
		/// Loads the <see cref="FormState"/> of a particular <paramref name="form"/> from the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to load the state.</param>
		/// <returns>Returns the loaded <see cref="FormState"/>.</returns>
		public FormState LoadState(IMansionWebContext context, Form form)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			return DoLoadState(context, form);
		}
		/// <summary>
		/// Loads the <see cref="FormState"/> of a particular <paramref name="form"/> from the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to load the state.</param>
		/// <returns>Returns the loaded <see cref="FormState"/>.</returns>
		protected abstract FormState DoLoadState(IMansionWebContext context, Form form);
		/// <summary>
		/// Stores the <see cref="FormState"/> of a particular <paramref name="form"/> in the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to store the state.</param>
		/// <param name="state">The <see cref="FormState"/> which to save.</param>
		public void StoreState(IMansionWebContext context, Form form, FormState state)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (state == null)
				throw new ArgumentNullException("state");
			DoStoreState(context, form, state);
		}
		/// <summary>
		/// Stores the <see cref="FormState"/> of a particular <paramref name="form"/> in the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to store the state.</param>
		/// <param name="state">The <see cref="FormState"/> which to save.</param>
		protected abstract void DoStoreState(IMansionWebContext context, Form form, FormState state);
		#endregion
		#region Advance Methods
		/// <summary>
		/// Advances the <paramref name="form"/> to the <paramref name="step"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being advanced.</param>
		/// <param name="step">The <see cref="Step"/> to which to advance.</param>
		public void AdvanceTo(IMansionWebContext context, Form form, Step step)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (step == null)
				throw new ArgumentNullException("step");
			DoAdvanceTo(context, form, step);
		}
		/// <summary>
		/// Advances the <paramref name="form"/> to the <paramref name="step"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being advanced.</param>
		/// <param name="step">The <see cref="Step"/> to which to advance.</param>
		protected abstract void DoAdvanceTo(IMansionWebContext context, Form form, Step step);
		#endregion
	}
}