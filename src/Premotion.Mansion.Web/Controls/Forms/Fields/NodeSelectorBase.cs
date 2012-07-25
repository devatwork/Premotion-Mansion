using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Represents the base class for controls which allow the user to select <see cref="Node"/>s.
	/// </summary>
	public abstract class NodeSelectorBase<TField, TValue> : Field<TValue> where TField : NodeSelectorBase<TField, TValue>
	{
		#region Nested type: NodeSelectorBaseFactoryTag
		/// <summary>
		/// Constructs <see cref="NodeSelectorBase{TField,TValue}"/>s.
		/// </summary>
		public abstract class NodeSelectorBaseFactoryTag : FieldFactoryTag<TField>
		{
			#region Overrides of ControlFactoryTag<NodeSelectorBase>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override TField Create(IMansionWebContext context, ControlDefinition definition)
			{
				// retrieve the selector properties
				var settingsTag = GetAlternativeChildTag<NodeSelectorPropertiesTag>();

				// create the control
				var control = DoCreate(context, definition);

				// merge the selector properties
				control.selectorProperties.Merge(settingsTag.GetAttributes(context));

				// set the selector properties
				control.selectorProperties.Set("controlId", definition.Id);
				control.LabelProperty = control.selectorProperties.Get(context, "labelProperty", "name");
				control.ValueProperty = control.selectorProperties.Get(context, "valueProperty", "guid");
				control.selectorProperties.Set("labelProperty", control.LabelProperty);
				control.selectorProperties.Set("valueProperty", control.ValueProperty);

				// return the created control
				return control;
			}

			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract TField DoCreate(IMansionWebContext context, ControlDefinition definition);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected NodeSelectorBase(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field{TValue}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			using (context.Stack.Push("SelectorProperties", SelectorProperties))
				base.DoRender(context, templateService);
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the selector properties.
		/// </summary>
		protected IPropertyBag SelectorProperties
		{
			get { return selectorProperties; }
		}
		/// <summary>
		/// Gets the name of the value property.
		/// </summary>
		protected string ValueProperty { get; private set; }
		/// <summary>
		/// Gets the name of the label property.
		/// </summary>
		protected string LabelProperty { get; private set; }
		#endregion
		#region Private Fields
		private readonly IPropertyBag selectorProperties = new PropertyBag();
		#endregion
	}
}