using System;
using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Buttons
{
	/// <summary>
	/// Represents a bar of buttons.
	/// </summary>
	public class ButtonBar : FormControlContainer
	{
		#region Nested type: ButtonBarFactoryTag
		/// <summary>
		/// Factory for <see cref="ButtonBar"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "buttonbar")]
		public class ButtonBarFactoryTag : FormControlFactoryTag<ButtonBar>
		{
			#region Overrides of ControlFactoryTag<Form>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override ButtonBar Create(MansionWebContext context, ControlDefinition definition)
			{
				// create the form
				return new ButtonBar(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ButtonBar(ControlDefinition definition) : base(definition)
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