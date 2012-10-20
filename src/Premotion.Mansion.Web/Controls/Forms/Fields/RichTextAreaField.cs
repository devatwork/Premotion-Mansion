using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a rich-text <see cref="Field{TValue}"/>.
	/// </summary>
	public class RichTextAreaField : Field<string>
	{
		#region Nested type: RichTextAreaFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "richTextArea")]
		public class RichTextAreaFactoryTag : FieldFactoryTag<RichTextAreaField>
		{
			#region Overrides of FieldFactoryTag<RichTextArea>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override RichTextAreaField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new RichTextAreaField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public RichTextAreaField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}