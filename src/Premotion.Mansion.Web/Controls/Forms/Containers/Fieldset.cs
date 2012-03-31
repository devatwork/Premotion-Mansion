using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Containers
{
	/// <summary>
	/// Represents a fieldset.
	/// </summary>
	public class Fieldset : FormControlContainer
	{
		#region Nested type: FieldsetFactoryTag
		/// <summary>
		/// This tags creates <see cref="Fieldset"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "fieldset")]
		public class FieldsetFactoryTag : FormControlFactoryTag<Fieldset>
		{
			#region Overrides of ControlFactoryTag<Step>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override Fieldset Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new Fieldset(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public Fieldset(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}