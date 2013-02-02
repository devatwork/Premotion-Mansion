using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// A node selector interface.
	/// </summary>
	public class MultiNodeSelector2Field : Field<string>
	{
		#region Nested type: MultiNodeSelector2FieldFactoryTag
		/// <summary>
		/// Constructs <see cref="NodeTreeSelectField"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "multiNodeSelector2")]
		public class MultiNodeSelector2FieldFactoryTag : FieldFactoryTag<MultiNodeSelector2Field>
		{
			#region Overrides of FieldFactoryTag<SingleNodeSelector2Field>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MultiNodeSelector2Field Create(IMansionWebContext context, ControlDefinition definition)
			{
				// get the node selector properties tag
				var nodeSelectorPropertiesTag = GetAlternativeChildTag<NodeSelectorPropertiesTag>();

				// create the control
				return new MultiNodeSelector2Field(nodeSelectorPropertiesTag.GetAttributes(context), definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a field.
		/// </summary>
		/// <param name="selectorProperties"></param>
		/// <param name="definition"></param>
		public MultiNodeSelector2Field(IPropertyBag selectorProperties, ControlDefinition definition) : base(definition)
		{
			// validatete arguments
			if (selectorProperties == null)
				throw new ArgumentNullException("selectorProperties");

			this.selectorProperties = selectorProperties;
		}
		#endregion
		#region Overrides of Control
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, Core.Templating.ITemplateService templateService)
		{
			// get the node selector properties
			using (context.Stack.Push("NodeSelectorProperties", selectorProperties))
				base.DoRender(context, templateService);
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag selectorProperties;
		#endregion
	}
}