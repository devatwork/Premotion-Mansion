using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// ShowsShows a warning message for the user.
	/// </summary>
	public class WarningAlert : AlertBase
	{
		#region Nested type: WarningAlertTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "WarningAlert")]
		public class WarningAlertTag : AlertBaseTag
		{
			#region Overrides of AlertBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new WarningAlert(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public WarningAlert(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}