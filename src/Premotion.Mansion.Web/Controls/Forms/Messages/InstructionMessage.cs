using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Messages
{
	/// <summary>
	/// Shows an instruction message for the user.
	/// </summary>
	public class InstructionMessage : MessageBase
	{
		#region Nested type: InstructionMessageTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "instructionMessage")]
		public class InstructionMessageTag : MessageBaseTag
		{
			#region Overrides of MessageBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MessageBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new InstructionMessage(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public InstructionMessage(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}