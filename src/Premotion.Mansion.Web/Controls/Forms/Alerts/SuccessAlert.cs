using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// Shows a success message for the user.
	/// </summary>
	public class SuccessAlert : AlertBase
	{
		#region Nested type: SuccessAlertTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "SuccessAlert")]
		public class SuccessAlertTag : AlertBaseTag
		{
			#region Overrides of AlertBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new SuccessAlert(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public SuccessAlert(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}