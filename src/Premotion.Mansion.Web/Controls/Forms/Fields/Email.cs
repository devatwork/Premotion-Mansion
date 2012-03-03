using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Web.Controls.Forms.Validation;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a e-mail textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class Email : Field<string>
	{
		#region Nested type: EmailFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "email")]
		public class EmailFactoryTag : FieldFactoryTag<Email>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Email Create(MansionWebContext context, ControlDefinition definition)
			{
				var field = new Email(definition);
				field.Add(new EmailFieldValidator(new PropertyBag()));
				return field;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Email(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}