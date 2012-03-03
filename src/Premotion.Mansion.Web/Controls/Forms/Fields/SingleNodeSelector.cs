using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Represents a control which allows to select a single <see cref="Node"/>.
	/// </summary>
	public class SingleNodeSelector : NodeSelectorBase
	{
		#region Nested type: NodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="SingleNodeSelector"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "singleNodeSelector")]
		public class NodeSelectorFactoryTag : NodeSelectorBaseFactoryTag
		{
			#region Overrides of NodeSelectorBaseFactoryTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override NodeSelectorBase DoCreate(MansionWebContext context, ControlDefinition definition)
			{
				return new SingleNodeSelector(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public SingleNodeSelector(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}