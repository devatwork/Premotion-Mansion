using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Represents a control which allows to select multiple <see cref="Node"/>s.
	/// </summary>
	public class MultiNodeSelector : NodeSelectorBase
	{
		#region Nested type: NodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="MultiNodeSelector"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "multiNodeSelector")]
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
				return new MultiNodeSelector(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public MultiNodeSelector(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}