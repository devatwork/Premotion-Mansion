using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Web.Controls.Forms.Validation;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a url textbox <see cref="Field{TValue}"/>.
	/// </summary>
	public class Url : Field<string>
	{
		#region Nested type: UrlFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Url"/>.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "url")]
		public class UrlFactoryTag : FieldFactoryTag<Url>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Url Create(MansionWebContext context, ControlDefinition definition)
			{
				var field = new Url(definition);
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
		public Url(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}