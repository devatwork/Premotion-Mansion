using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements a hidden field.
	/// </summary>
	public class Hidden : FormControl
	{
		#region Nested type: HiddenFactoryTag
		/// <summary>
		/// This tag creates a <see cref="Hidden"/> field.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "hidden")]
		public class HiddenFactoryTag : FormControlFactoryTag<Hidden>
		{
			#region Overrides of FieldFactoryTag<Textbox>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Hidden Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new Hidden(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Hidden(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of FormControl
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// set the value
			var name = Definition.Properties.Get<string>(context, "name");
			var value = Definition.Properties.Get<object>(context, "value");
			form.State.FieldProperties.Set(name, value);
		}
		#endregion
	}
}