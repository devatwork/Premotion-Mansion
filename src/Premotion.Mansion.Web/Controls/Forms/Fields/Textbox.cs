using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class Textbox : Field<string>
	{
		#region Nested type: TextboxFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "textbox")]
		public class TextboxFactoryTag : FieldFactoryTag<Textbox>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Textbox Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new Textbox(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Textbox(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}