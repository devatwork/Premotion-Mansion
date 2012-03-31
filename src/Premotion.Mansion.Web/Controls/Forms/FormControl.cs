using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms
{
	/// <summary>
	/// Implements the base class for all form controls.
	/// </summary>
	public abstract class FormControl : Container
	{
		#region Form Control Factory Tag
		/// <summary>
		/// Base tag for <see cref="ScriptTag"/>s creating <see cref="FormControl"/>s.
		/// </summary>
		public abstract class FormControlFactoryTag<TControl> : ControlFactoryTag<TControl> where TControl : FormControl
		{
			#region Overrides of ControlFactoryTag<TControl>
			/// <summary>
			/// Executes the created <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="control">The control which to execute.</param>
			protected override void Execute(IMansionWebContext context, TControl control)
			{
				// add the control to the form control container
				FormControlContainer container;
				if (context.TryFindControl(out container))
					container.AddFormControl(control);

				base.Execute(context, control);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected FormControl(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Lifecycle Methods
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		public void Initialize(IMansionWebContext context, Form form)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			DoInitialize(context, form);
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected virtual void DoInitialize(IMansionWebContext context, Form form)
		{
			// do nothing
		}
		/// <summary>
		/// Validates the state of this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		/// <param name="results">The <see cref="ValidationResults"/> in which the validation results are stored.</param>
		public void Validate(IMansionWebContext context, Form form, ValidationResults results)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (form == null)
				throw new ArgumentNullException("form");
			if (results == null)
				throw new ArgumentNullException("results");
			DoValidate(context, form, results);
		}
		/// <summary>
		/// Validates the state of this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		/// <param name="results">The <see cref="ValidationResults"/> in which the validation results are stored.</param>
		protected virtual void DoValidate(IMansionWebContext context, Form form, ValidationResults results)
		{
			// loop over all the validation rules
			foreach (var rule in ValidationRules)
				rule.Validate(context, form, this, results);
		}
		#endregion
		#region Validation Rule Methods
		/// <summary>
		/// Adds a new <paramref name="rule"/> to this control.
		/// </summary>
		/// <param name="rule">The <see cref="ValidationRule"/> which to add.</param>
		public virtual void Add(ValidationRule rule)
		{
			// validate arguments
			if (rule == null)
				throw new ArgumentNullException("rule");
			validationRules.Add(rule);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the validation rules of this control.
		/// </summary>
		protected IEnumerable<ValidationRule> ValidationRules
		{
			get { return validationRules; }
		}
		#endregion
		#region Private Fields
		private readonly List<ValidationRule> validationRules = new List<ValidationRule>();
		#endregion
	}
}