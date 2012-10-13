using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a checkbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class CheckboxField : Field<bool>
	{
		#region Nested type: CheckboxTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "checkbox")]
		public class CheckboxTag : FieldFactoryTag<CheckboxField>
		{
			#region Overrides of FieldFactoryTag<Checkbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override CheckboxField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new CheckboxField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public CheckboxField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field
		/// <summary>
		/// Gets a flag indicating whether this field has a value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when the field is considered to have value, otherwise false.</returns>
		public override bool HasValue(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			return GetValue(context);
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// if the form is posted back make sure the boolean is set when the checkbox was unchecked
			if (form.State.IsPostback)
				form.State.FieldProperties.Set(Name, form.State.FieldProperties.Get(context, Name, false));

			base.DoInitialize(context, form);
		}
		#endregion
	}
}