using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a select <see cref="ListField{TValue}"/>.
	/// </summary>
	public class Selectbox : ListField<string>
	{
		#region Nested type: SelectboxFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Textbox"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "selectbox")]
		public class SelectboxFactoryTag : ListFieldFactoryTag<Selectbox>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			/// <param name="mappingStrategy">The <see cref="RowMappingStrategy"/> used by the list field.</param>
			protected override Selectbox Create(MansionWebContext context, ControlDefinition definition, RowMappingStrategy mappingStrategy)
			{
				return new Selectbox(definition, mappingStrategy);
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
		public Selectbox(ControlDefinition definition, RowMappingStrategy mappingStrategy) : base(definition, mappingStrategy)
		{
		}
		#endregion
	}
}