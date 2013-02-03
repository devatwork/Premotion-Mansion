using System;
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

				// create the setttings
				var settings = new PropertyBag {
					{"valueProperty", "guid"},
					{"labelProperty", "name"}
				}.Merge(settingsTag.GetAttributes(context));

				// return the created control
				return DoCreate(context, settings, definition);
			}

			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="settings"></param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected abstract TField DoCreate(IMansionWebContext context, IPropertyBag settings, ControlDefinition definition);
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="settings">The settings of this selector.</param>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		protected NodeSelectorBase(IPropertyBag settings, ControlDefinition definition) : base(definition)
		{
			// validate arguments
			if (settings == null)
				throw new ArgumentNullException("settings");

			// set values
			this.settings = settings;
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
			// render the templates
			templateService.Render(context, "NodeSelectorTemplates").Dispose();

			// render the control
			using (context.Stack.Push("NodeSelectorProperties", settings))
				base.DoRender(context, templateService);
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag settings = new PropertyBag();
		#endregion
	}
}