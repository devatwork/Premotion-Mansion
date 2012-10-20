using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a Readonly <see cref="Field{TValue}"/>.
	/// </summary>
	public class ReadonlyField : Field<string>
	{
		#region Nested type: ReadonlyFactoryTag
		/// <summary>
		/// This tag creates a <see cref="ReadonlyField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "readonly")]
		public class ReadonlyFactoryTag : FieldFactoryTag<ReadonlyField>
		{
			#region Overrides of FieldFactoryTag<Readonly>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override ReadonlyField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new ReadonlyField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public ReadonlyField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}