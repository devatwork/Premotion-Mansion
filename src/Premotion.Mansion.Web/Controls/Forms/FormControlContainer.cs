using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Base class for <see cref="FormControl"/>s which contain other <see cref="FormControl"/>s.
	/// </summary>
	public abstract class FormControlContainer : FormControl
	{
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected FormControlContainer(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Add Control Methods
		/// <summary>
		/// Adds a <paramref name="control"/> to this container.
		/// </summary>
		/// <param name="control">The <see cref="FormControl"/> which to add.</param>
		public void AddFormControl(FormControl control)
		{
			// validate arguments
			if (control == null)
				throw new ArgumentNullException("control");

			// check if the control is allowed
			var controlType = control.GetType();
			if (!IsControlAllowed(controlType))
				throw new InvalidOperationException(string.Format("The control of type '{0}' is not allowed as a direct child of '{1}'", controlType, GetType()));

			// add the control to the list
			formControls.Add(control);
		}
		/// <summary>
		/// Checks whether <paramref name="controlType"/> is allowed in this container.
		/// </summary>
		/// <param name="controlType">The type of control which to check.</param>
		/// <returns>Returns true when the control is allowed, otherwise false.</returns>
		protected virtual bool IsControlAllowed(Type controlType)
		{
			return !typeof (Form).IsAssignableFrom(controlType) && !typeof (Step).IsAssignableFrom(controlType);
		}
		#endregion
		#region Overrides of FormControl
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// loop over all the controls
			foreach (var control in FormControls)
				control.Initialize(context, form);
		}
		/// <summary>
		/// Validates the state of this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		/// <param name="results">The <see cref="ValidationResults"/> in which the validation results are stored.</param>
		protected override void DoValidate(IMansionWebContext context, Form form, ValidationResults results)
		{
			// loop over all the controls
			foreach (var control in FormControls)
				control.Validate(context, form, results);
			base.DoValidate(context, form, results);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <see cref="FormControl"/>s contained by this container.
		/// </summary>
		protected IEnumerable<FormControl> FormControls
		{
			get { return formControls; }
		}
		#endregion
		#region Private Fields
		private readonly List<FormControl> formControls = new List<FormControl>();
		#endregion
	}
}