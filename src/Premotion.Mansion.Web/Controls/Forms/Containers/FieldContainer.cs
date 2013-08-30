using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Containers
{
	/// <summary>
	/// Represents a group of <see cref="Control"/>s.
	/// </summary>
	public class FieldContainer : FormControlContainer
	{
		#region Nested type: FieldContainerFactoryTag
		/// <summary>
		/// Constructs <see cref="FieldContainer"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "fieldContainer")]
		public class FieldContainerFactoryTag : FormControlFactoryTag<FieldContainer>
		{
			#region Overrides of FormControlFactoryTag<FieldContainer>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override FieldContainer Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new FieldContainer(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public FieldContainer(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}