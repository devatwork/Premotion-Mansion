using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a Readonly <see cref="Field{TValue}"/>.
	/// </summary>
	public class Readonly : Field<string>
	{
		#region Nested type: ReadonlyFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Readonly"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "readonly")]
		public class ReadonlyFactoryTag : FieldFactoryTag<Readonly>
		{
			#region Overrides of FieldFactoryTag<Readonly>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Readonly Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new Readonly(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Readonly(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}