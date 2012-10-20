using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a textarea <see cref="Field{TValue}"/>.
	/// </summary>
	public class TextareaField : Field<string>
	{
		#region Nested type: TextareaFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TextareaField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "textarea")]
		public class TextareaFactoryTag : FieldFactoryTag<TextareaField>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TextareaField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new TextareaField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public TextareaField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}