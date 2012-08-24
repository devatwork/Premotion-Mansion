using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// Implements a simple message control.
	/// </summary>
	public abstract class AlertBase : FormControl
	{
		#region Nested type: AlertBaseTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		public abstract class AlertBaseTag : FormControlFactoryTag<AlertBase>
		{
			#region Overrides of FieldFactoryTag<Message>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override sealed AlertBase Create(IMansionWebContext context, ControlDefinition definition)
			{
				// set the message
				definition.Properties.Set("message", GetContent<string>(context));

				// create the control
				return CreateMessageControl(context, definition);
			}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected AlertBase(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}