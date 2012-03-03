﻿using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a password <see cref="Field{TValue}"/>.
	/// </summary>
	public class Password : Field<string>
	{
		#region Nested type: PasswordFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Password"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "password")]
		public class PasswordFactoryTag : FieldFactoryTag<Password>
		{
			#region Overrides of FieldFactoryTag<Password>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Password Create(MansionWebContext context, ControlDefinition definition)
			{
				return new Password(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Password(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}