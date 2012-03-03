using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a date <see cref="Field{TValue}"/>.
	/// </summary>
	public class Date : Field<DateTime>
	{
		#region Nested type: DateFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "date")]
		public class DateFactoryTag : FieldFactoryTag<Date>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Date Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Date(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Date(ControlDefinition definition) : base(definition)
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

			// try to get the value
			var value = GetValue(context);

			// check if the value is set
			return value != DateTime.MinValue;
		}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(MansionWebContext context, Form form)
		{
			base.DoInitialize(context, form);

			// if the datetime is null, remove it from the properties
			DateTime date;
			var value = form.State.FieldProperties.TryGetAndRemove(context, Name, out date) && date != DateTime.MinValue ? (object) date : null;
			form.State.FieldProperties.Set(Name, value);
		}
		#endregion
	}
}