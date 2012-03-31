using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a number <see cref="Field{TValue}"/>.
	/// </summary>
	public class Number : Field<int>
	{
		#region Nested type: NumberTag
		/// <summary>
		/// This tag creates a <see cref="Number"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "number")]
		public class NumberTag : FieldFactoryTag<Number>
		{
			#region Overrides of FieldFactoryTag<Number>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Number Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new Number(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Number(ControlDefinition definition) : base(definition)
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
			return true;
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// if the form is posted back make sure the boolean is set when the Number was unchecked
			if (form.State.IsPostback)
				form.State.FieldProperties.Set(Name, form.State.FieldProperties.Get(context, Name, 0));

			base.DoInitialize(context, form);
		}
		#endregion
	}
}