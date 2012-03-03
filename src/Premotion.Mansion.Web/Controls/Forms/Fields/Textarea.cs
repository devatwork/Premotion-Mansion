using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a textarea <see cref="Field{TValue}"/>.
	/// </summary>
	public class Textarea : Field<string>
	{
		#region Nested type: TextareaFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textarea"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "textarea")]
		public class TextareaFactoryTag : FieldFactoryTag<Textarea>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Textarea Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Textarea(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Textarea(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}