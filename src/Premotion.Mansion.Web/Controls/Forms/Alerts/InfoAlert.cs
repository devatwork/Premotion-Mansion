using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// Shows an info message for the user.
	/// </summary>
	public class InfoAlert : AlertBase
	{
		#region Nested type: InfoAlertTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "InfoAlert")]
		public class InfoAlertTag : AlertBaseTag
		{
			#region Overrides of AlertBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new InfoAlert(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public InfoAlert(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}