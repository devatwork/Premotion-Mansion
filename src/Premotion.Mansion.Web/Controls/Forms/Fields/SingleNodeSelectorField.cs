using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Allows the user to select a single <see cref="Node"/>.
	/// </summary>
	public class SingleNodeSelectorField : NodeSelectorBase<SingleNodeSelectorField, string>
	{
		#region Nested type: SingleNodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="SingleNodeSelectorField"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "singleNodeSelector")]
		public class SingleNodeSelectorFactoryTag : NodeSelectorBaseFactoryTag
		{
			#region Overrides of NodeSelectorBaseFactoryTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="settings"></param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override SingleNodeSelectorField DoCreate(IMansionWebContext context, IPropertyBag settings, ControlDefinition definition)
			{
				return new SingleNodeSelectorField(settings, definition);
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
		public SingleNodeSelectorField(IPropertyBag settings, ControlDefinition definition) : base(settings, definition)
		{
		}
		#endregion
	}
}