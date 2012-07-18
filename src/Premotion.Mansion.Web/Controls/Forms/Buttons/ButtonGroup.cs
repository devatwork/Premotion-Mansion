using System;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Buttons
{
	/// <summary>
	/// Represents a group of buttons.
	/// </summary>
	public class ButtonGroup : FormControlContainer
	{
		#region Nested type: ButtonGroupFactoryTag
		/// <summary>
		/// Factory for <see cref="ButtonGroup"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "buttonGroup")]
		public class ButtonGroupFactoryTag : FormControlFactoryTag<ButtonGroup>
		{
			#region Overrides of ControlFactoryTag<Form>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override ButtonGroup Create(IMansionWebContext context, ControlDefinition definition)
			{
				// create the form
				return new ButtonGroup(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ButtonGroup(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of FormControlContainer
		/// <summary>
		/// Checks whether <paramref name="controlType"/> is allowed in this container.
		/// </summary>
		/// <param name="controlType">The type of control which to check.</param>
		/// <returns>Returns true when the control is allowed, otherwise false.</returns>
		protected override bool IsControlAllowed(Type controlType)
		{
			return typeof (Button).IsAssignableFrom(controlType);
		}
		#endregion
	}
}