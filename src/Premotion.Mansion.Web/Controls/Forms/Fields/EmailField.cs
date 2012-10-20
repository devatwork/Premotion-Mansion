using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Validation;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a e-mail textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class EmailField : Field<string>
	{
		#region Nested type: EmailFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "email")]
		public class EmailFactoryTag : FieldFactoryTag<EmailField>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override EmailField Create(IMansionWebContext context, ControlDefinition definition)
			{
				var field = new EmailField(definition);
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
		public EmailField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}