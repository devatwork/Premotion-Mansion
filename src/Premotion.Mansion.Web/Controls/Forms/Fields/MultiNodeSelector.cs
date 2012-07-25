using System;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Templating;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Allows the user to select a single <see cref="Node"/>.
	/// </summary>
	public class MultiNodeSelector : NodeSelectorBase<MultiNodeSelector, string>
	{
		#region Nested type: MultiNodeSelectorFactoryTag
		/// <summary>
		/// Constructs <see cref="SingleNodeSelector"/>s.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "multiNodeSelector")]
		public class MultiNodeSelectorFactoryTag : NodeSelectorBaseFactoryTag
		{
			#region Overrides of NodeSelectorBaseFactoryTag
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override MultiNodeSelector DoCreate(IMansionWebContext context, ControlDefinition definition)
			{
				return new MultiNodeSelector(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructors this control.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public MultiNodeSelector(ControlDefinition definition) : base(definition)
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
			// set the field value
			var selectedItems = GetValue(context) ?? string.Empty;
			Definition.Properties.Set("Value", selectedItems);

			// render the field
			using (context.Stack.Push("SelectorProperties", SelectorProperties))
			using (context.Stack.Push("FieldControl", PropertyBagAdapterFactory.Adapt<Field>(context, this)))
			using (templateService.Render(context, "FieldContainer"))
			using (templateService.Render(context, GetType().Name + "Control"))
			{
				// loop over all the values
				var repository = context.Repository;
				foreach (var value in selectedItems.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(c => !string.IsNullOrEmpty(c)))
				{
					// retrieve the selected node
					var selectedNode = repository.RetrieveSingle(context, new PropertyBag
					                                                      {
					                                                      	{ValueProperty, value}
					                                                      });

					// turn into label/value row
					var row = new PropertyBag
					          {
					          	{"label", selectedNode.Get(context, LabelProperty, selectedNode.Get<string>(context, ValueProperty))},
					          	{"value", selectedNode.Get<object>(context, ValueProperty)}
					          };

					// render the selected value
					using (context.Stack.Push("Row", row))
						templateService.Render(context, GetType().Name + "ControlOption").Dispose();
				}
			}
		}
		#endregion
	}
}