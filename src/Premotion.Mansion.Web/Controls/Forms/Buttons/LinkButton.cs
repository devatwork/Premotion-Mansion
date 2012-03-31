using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Buttons
{
	/// <summary>
	/// Represents a clickable button which does not trigger a form submit.
	/// </summary>
	public class LinkButton : Button
	{
		#region Nested type: DialogButtonFactoryTag
		/// <summary>
		/// Factory for <see cref="LinkButton"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "linkButton")]
		public class DialogButtonFactoryTag : FormControlFactoryTag<LinkButton>
		{
			#region Overrides of ControlFactoryTag<LinkButton>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override LinkButton Create(IMansionWebContext context, ControlDefinition definition)
			{
				// check required values
				GetRequiredAttribute<string>(context, "action");
				GetRequiredAttribute<string>(context, "label");

				// create the form
				return new LinkButton(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public LinkButton(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}