﻿using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Web.Controls.Forms.Fields;

namespace Premotion.Mansion.Web.Controls.Forms.Messages
{
	/// <summary>
	/// ShowsShows a warning message for the user.
	/// </summary>
	public class WarningMessage : MessageBase
	{
		#region Nested type: WarningMessageTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "warningMessage")]
		public class WarningMessageTag : MessageBaseTag
		{
			#region Overrides of MessageBaseTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MessageBase CreateMessageControl(MansionWebContext context, ControlDefinition definition)
			{
				return new WarningMessage(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public WarningMessage(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}