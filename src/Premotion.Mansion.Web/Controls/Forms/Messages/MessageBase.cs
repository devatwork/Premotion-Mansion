using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Messages
{
	/// <summary>
	/// Implements a simple message control.
	/// </summary>
	public abstract class MessageBase : FormControl
	{
		#region Nested type: MessageBaseTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		public abstract class MessageBaseTag : FormControlFactoryTag<MessageBase>
		{
			#region Overrides of FieldFactoryTag<Message>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override sealed MessageBase Create(MansionWebContext context, ControlDefinition definition)
			{
				// set the message
				definition.Properties.Set("message", GetContent<string>(context));

				// create the control
				return CreateMessageControl(context, definition);
			}
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract MessageBase CreateMessageControl(MansionWebContext context, ControlDefinition definition);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected MessageBase(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}