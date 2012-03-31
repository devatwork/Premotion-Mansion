using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Buttons
{
	/// <summary>
	/// Represents a clickable button which triggers an action.
	/// </summary>
	public class Button : FormControl
	{
		#region Nested type: ButtonFactoryTag
		/// <summary>
		/// Factory for <see cref="Button"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "button")]
		public class ButtonFactoryTag : FormControlFactoryTag<Button>
		{
			#region Overrides of ControlFactoryTag<Form>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Button Create(IMansionWebContext context, ControlDefinition definition)
			{
				// check required values
				GetRequiredAttribute<string>(context, "action");
				GetRequiredAttribute<string>(context, "label");

				// create the form
				return new Button(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Button(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}