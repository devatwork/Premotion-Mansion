using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a select <see cref="ListField{TValue}"/>.
	/// </summary>
	public class SelectboxField : ListField<string>
	{
		#region Nested type: SelectboxFactoryTag
		/// <summary>
		/// This tag creates a <see cref="TextboxField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "selectbox")]
		public class SelectboxFactoryTag : ListFieldFactoryTag<SelectboxField>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
			protected override SelectboxField Create(IMansionWebContext context, ControlDefinition definition, RowMappingStrategy mappingStrategy)
			{
				return new SelectboxField(definition, mappingStrategy);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
		public SelectboxField(ControlDefinition definition, RowMappingStrategy mappingStrategy) : base(definition, mappingStrategy)
		{
		}
		#endregion
	}
}