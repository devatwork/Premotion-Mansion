using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Messages
{
	/// <summary>
	/// Shows an error message for the user.
	/// </summary>
	public class ErrorMessage : MessageBase
	{
		#region Nested type: ErrorMessageTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "ErrorMessage")]
		public class ErrorMessageTag : MessageBaseTag
		{
			#region Overrides of MessageBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MessageBase CreateMessageControl(MansionWebContext context, ControlDefinition definition)
			{
				return new ErrorMessage(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ErrorMessage(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}