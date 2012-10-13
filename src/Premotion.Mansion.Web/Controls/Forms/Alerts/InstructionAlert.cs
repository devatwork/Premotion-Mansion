using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// Shows an instruction message for the user.
	/// </summary>
	public class InstructionAlert : AlertBase
	{
		#region Nested type: InstructionAlertTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "InstructionAlert")]
		public class InstructionAlertTag : AlertBaseTag
		{
			#region Overrides of AlertBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new InstructionAlert(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public InstructionAlert(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}