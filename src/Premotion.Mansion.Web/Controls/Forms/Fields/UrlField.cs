using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Web.Controls.Forms.Validation;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a url textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class UrlField : Field<string>
	{
		#region Nested type: UrlFactoryTag
		/// <summary>
		/// This tag creates a <see cref="UrlField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "url")]
		public class UrlFactoryTag : FieldFactoryTag<UrlField>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override UrlField Create(IMansionWebContext context, ControlDefinition definition)
			{
				var field = new UrlField(definition);
				field.Add(new UrlFieldValidator(new PropertyBag()));
				return field;
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public UrlField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}