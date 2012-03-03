using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a checkbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class Checkbox : Field<bool>
	{
		#region Nested type: CheckboxTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "checkbox")]
		public class CheckboxTag : FieldFactoryTag<Checkbox>
		{
			#region Overrides of FieldFactoryTag<Checkbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Checkbox Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Checkbox(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Checkbox(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field
		/// <summary>
		/// Gets a flag indicating whether this field has a value.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <returns>Returns true when the field is considered to have value, otherwise false.</returns>
		public override bool HasValue(IContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return GetValue(context);
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(MansionWebContext context, Form form)
		{
			// if the form is posted back make sure the boolean is set when the checkbox was unchecked
			if (form.State.IsPostback)
				form.State.FieldProperties.Set(Name, form.State.FieldProperties.Get(context, Name, false));

			base.DoInitialize(context, form);
		}
		#endregion
	}
}