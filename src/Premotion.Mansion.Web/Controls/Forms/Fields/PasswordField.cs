using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a password <see cref="Field{TValue}"/>.
	/// </summary>
	public class PasswordField : Field<string>
	{
		#region Nested type: PasswordFactoryTag
		/// <summary>
		/// This tag creates a <see cref="PasswordField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "password")]
		public class PasswordFactoryTag : FieldFactoryTag<PasswordField>
		{
			#region Overrides of FieldFactoryTag<Password>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override PasswordField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new PasswordField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public PasswordField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}