using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a Range <see cref="Field{TValue}"/>.
	/// </summary>
	public class Range : Field<int>
	{
		#region Nested type: RangeTag
		/// <summary>
		/// This tag creates a <see cref="Range"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "range")]
		public class RangeTag : FieldFactoryTag<Range>
		{
			#region Overrides of FieldFactoryTag<Range>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Range Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Range(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Range(ControlDefinition definition) : base(definition)
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
			return true;
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(MansionWebContext context, Form form)
		{
			// if the form is posted back make sure the boolean is set when the Range was unchecked
			if (form.State.IsPostback)
				form.State.FieldProperties.Set(Name, form.State.FieldProperties.Get(context, Name, 0));

			base.DoInitialize(context, form);
		}
		#endregion
	}
}