using System;
using System.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Represents the base class for controls which allow the user to select <see cref="Node"/>s.
	/// </summary>
	public abstract class NodeSelectorBase : Field<string>
	{
		#region Constants
		private static readonly string[] propertyBlacklist = new[] {"name", "label", "id", "displayValue", "isRequired"};
		#endregion
		#region Nested type: NodeSelectorBaseFactoryTag
		/// <summary>
		/// Constructs <see cref="NodeSelectorBase"/>s.
		/// </summary>
		public abstract class NodeSelectorBaseFactoryTag : FieldFactoryTag<NodeSelectorBase>
		{
			#region Overrides of ControlFactoryTag<NodeSelectorBase>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override NodeSelectorBase Create(IMansionWebContext context, ControlDefinition definition)
			{
				// make sure the value property is set
				string valueProperty;
				if (!definition.Properties.TryGet(context, "valueProperty", out valueProperty))
				{
					valueProperty = "guid";
					definition.Properties.Set("valueProperty", valueProperty);
				}

				// make sure the label property is set
				string labelProperty;
				if (!definition.Properties.TryGet(context, "labelProperty", out labelProperty))
				{
					labelProperty = "name";
					definition.Properties.Set("labelProperty", labelProperty);
				}

				// create the control
				var control = DoCreate(context, definition);

				// set the properties
				control.ValueProperty = valueProperty;
				control.LabelProperty = labelProperty;

				return control;
			}

			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract NodeSelectorBase DoCreate(IMansionWebContext context, ControlDefinition definition);
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
		#region Overrides of Field{string}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			//  first allow base to initialize
			base.DoInitialize(context, form);

			// if there is no value do not display it
			if (!HasValue(context))
				return;
			var currentValue = GetValue(context);
			if (string.IsNullOrEmpty(currentValue))
				return;

			// retrieve the corresponding node
			var repository = context.Repository;
			var nodeQuery = repository.ParseQuery(context, new PropertyBag
			                                               {
			                                               	{ValueProperty, currentValue}
			                                               });
			var node = repository.RetrieveSingle(context, nodeQuery);

			// if no node was found clear the value of this field
			if (node == null)
			{
				ClearValue();
				return;
			}

			// set the display value
			Definition.Properties.Set("displayValue", node.Get(context, LabelProperty, node.Get<string>(context, ValueProperty)));
		}
		/// <summary>
		/// Render this control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="templateService">The <see cref="ITemplateService"/>.</param>
		protected override void DoRender(IMansionWebContext context, ITemplateService templateService)
		{
			// create the selector properties
			var selectorProperties = new PropertyBag(Definition.Properties.Where(x => x.Value != null && !propertyBlacklist.Contains(x.Key, StringComparer.OrdinalIgnoreCase)));
			selectorProperties.Set("controlId", Definition.Id);

			using (context.Stack.Push("SelectorProperties", selectorProperties, false))
				base.DoRender(context, templateService);
		}
		#endregion
		#region Properties
		private string ValueProperty { get; set; }
		private string LabelProperty { get; set; }
		#endregion
	}
}