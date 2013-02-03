using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Allows the user to select a single <see cref="Node"/>.
	/// </summary>
	public class MultiNodeSelectorField : NodeSelectorBase<MultiNodeSelectorField, string>
	{
		#region Nested type: MultiNodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="SingleNodeSelectorField"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "multiNodeSelector")]
		public class MultiNodeSelectorFactoryTag : NodeSelectorBaseFactoryTag
		{
			#region Overrides of NodeSelectorBaseFactoryTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="settings"></param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MultiNodeSelectorField DoCreate(IMansionWebContext context, IPropertyBag settings, ControlDefinition definition)
			{
				return new MultiNodeSelectorField(settings, definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructors this control.
		/// </summary>
		/// <param name="settings">The node selector settings.</param>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public MultiNodeSelectorField(IPropertyBag settings, ControlDefinition definition) : base(settings, definition)
		{
		}
		#endregion
	}
}