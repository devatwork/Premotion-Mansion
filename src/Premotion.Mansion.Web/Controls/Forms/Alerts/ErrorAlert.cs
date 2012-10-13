using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Alerts
{
	/// <summary>
	/// Shows an error message for the user.
	/// </summary>
	public class ErrorAlert : AlertBase
	{
		#region Nested type: ErrorAlertTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "ErrorAlert")]
		public class ErrorAlertTag : AlertBaseTag
		{
			#region Overrides of AlertBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override AlertBase CreateMessageControl(IMansionWebContext context, ControlDefinition definition)
			{
				return new ErrorAlert(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ErrorAlert(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}