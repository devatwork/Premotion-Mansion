using Premotion.Mansion.Core.Attributes;

namespace Premotion.Mansion.Web.Controls.Forms.Containers
{
	/// <summary>
	/// Represents a collapsible fieldset.
	/// </summary>
	public class CollapsibleFieldset : FormControlContainer
	{
		#region Nested type: CollapsibleFieldsetFactoryTag
		/// <summary>
		/// This tags creates <see cref="CollapsibleFieldset"/>s.
		/// </summary>
		[Named(Constants.FormTagNamespaceUri, "collapsibleFieldset")]
		public class CollapsibleFieldsetFactoryTag : FormControlFactoryTag<CollapsibleFieldset>
		{
			#region Overrides of ControlFactoryTag<Step>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override CollapsibleFieldset Create(MansionWebContext context, ControlDefinition definition)
			{
				return new CollapsibleFieldset(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public CollapsibleFieldset(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
	}
}